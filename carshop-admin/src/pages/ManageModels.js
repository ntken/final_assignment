import { useEffect, useState } from "react";
import axios from "axios";
import { Link } from "react-router-dom";
import ConfirmPopup from "../components/ConfirmPopup";
import "../styles.css";

const ManageModels = () => {
  const [models, setModels] = useState([]);
  const [editingId, setEditingId] = useState(null);
  const [newModelName, setNewModelName] = useState("");
  const [isPopupOpen, setIsPopupOpen] = useState(false);
  const [selectedModelId, setSelectedModelId] = useState(null);
  const [newModel, setNewModel] = useState("");

  useEffect(() => {
    fetchModels();
  }, []);

  const fetchModels = async () => {
    const token = localStorage.getItem("token");
    try {
      const response = await axios.get("http://localhost:5237/models", {
        headers: { Authorization: `Bearer ${token}` },
      });
      setModels(response.data);
    } catch (error) {
      console.error("Error fetching models:", error);
    }
  };

  const handleAddModel = async () => {
    const token = localStorage.getItem("token");
    if (!newModel.trim()) {
      alert("Model name cannot be empty!");
      return;
    }
    try {
      await axios.post(
        "http://localhost:5237/models",
        { name: newModel },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setNewModel("");
      fetchModels();
    } catch (error) {
      console.error("Error adding model:", error);
    }
  };

  const handleDeleteClick = (modelId) => {
    setSelectedModelId(modelId);
    setIsPopupOpen(true);
  };

  const handleConfirmDelete = async () => {
    const token = localStorage.getItem("token");
    try {
      await axios.delete(`http://localhost:5237/models/${selectedModelId}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      fetchModels();
    } catch (error) {
      console.error("Error deleting model:", error);
    }
    setIsPopupOpen(false);
  };

  const handleCancelDelete = () => {
    setIsPopupOpen(false);
    setSelectedModelId(null);
  };

  const handleEditClick = (id, currentName) => {
    setEditingId(id);
    setNewModelName(currentName);
  };

  const handleSaveClick = async (id) => {
    const token = localStorage.getItem("token");
    try {
      await axios.put(
        `http://localhost:5237/models/${id}`,
        { name: newModelName },
        { headers: { Authorization: `Bearer ${token}` }
      });
      setEditingId(null);
      fetchModels();
    } catch (error) {
      console.error("Error updating model:", error);
    }
  };

  return (
    <div>
      <Link to="/" className="back-to-dashboard">← Back to Dashboard</Link>
      <h2>Manage Models</h2>
      <div className="form-container">
        <input
          type="text"
          value={newModel}
          onChange={(e) => setNewModel(e.target.value)}
          placeholder="Enter new model name"
        />
        <button onClick={handleAddModel} className="add-btn">Add Model</button>
      </div>
      <div className="table-wrapper">
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Name</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {models.map((model) => (
              <tr key={model.id}>
                <td>{model.id}</td>
                <td>
                  {editingId === model.id ? (
                    <input
                      type="text"
                      value={newModelName}
                      onChange={(e) => setNewModelName(e.target.value)}
                    />
                  ) : (
                    model.name
                  )}
                </td>
                <td>
                  {editingId === model.id ? (
                    <button onClick={() => handleSaveClick(model.id)} className="edit-btn">Save</button>
                  ) : (
                    <button onClick={() => handleEditClick(model.id, model.name)} className="edit-btn">
                      Edit
                    </button>
                  )}
                  <button onClick={() => handleDeleteClick(model.id)} className="delete-btn">Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      {isPopupOpen && (
        <ConfirmPopup
          message="Are you sure you want to delete this model?"
          onCancel={handleCancelDelete}
          onConfirm={handleConfirmDelete}
        />
      )}
    </div>
  );
};

export default ManageModels;
