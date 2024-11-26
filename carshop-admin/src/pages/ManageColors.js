import { useEffect, useState } from "react";
import axios from "axios";
import { Link } from "react-router-dom";
import ConfirmPopup from "../components/ConfirmPopup";
import "../styles.css";

const ManageColors = () => {
  const [colors, setColors] = useState([]);
  const [editingId, setEditingId] = useState(null);
  const [newColorName, setNewColorName] = useState("");
  const [isPopupOpen, setIsPopupOpen] = useState(false);
  const [selectedColorId, setSelectedColorId] = useState(null);
  const [newColor, setNewColor] = useState("");

  useEffect(() => {
    fetchColors();
  }, []);

  const fetchColors = async () => {
    const token = localStorage.getItem("token");
    try {
      const response = await axios.get("http://localhost:5237/colors", {
        headers: { Authorization: `Bearer ${token}` },
      });
      setColors(response.data);
    } catch (error) {
      console.error("Error fetching colors:", error);
    }
  };

  const handleAddColor = async () => {
    const token = localStorage.getItem("token");
    if (!newColor.trim()) {
      alert("Color name cannot be empty!");
      return;
    }
    try {
      await axios.post(
        "http://localhost:5237/colors",
        { name: newColor },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setNewColor("");
      fetchColors();
    } catch (error) {
      console.error("Error adding color:", error);
    }
  };

  const handleDeleteClick = (colorId) => {
    setSelectedColorId(colorId);
    setIsPopupOpen(true);
  };

  const handleConfirmDelete = async () => {
    const token = localStorage.getItem("token");
    try {
      await axios.delete(`http://localhost:5237/colors/${selectedColorId}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      fetchColors();
    } catch (error) {
      console.error("Error deleting color:", error);
    }
    setIsPopupOpen(false);
  };

  const handleCancelDelete = () => {
    setIsPopupOpen(false);
    setSelectedColorId(null);
  };

  const handleEditClick = (id, currentName) => {
    setEditingId(id);
    setNewColorName(currentName);
  };

  const handleSaveClick = async (id) => {
    const token = localStorage.getItem("token");
    try {
      await axios.put(
        `http://localhost:5237/colors/${id}`,
        { name: newColorName },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setEditingId(null);
      fetchColors();
    } catch (error) {
      console.error("Error updating color:", error);
    }
  };

  return (
    <div>
      <div>
        <Link to="/" className="back-to-dashboard">
          ← Back to Dashboard
        </Link>
      </div>
      <h2>Manage Colors</h2>
      <div style={{ marginBottom: "20px" }}>
        <input
          type="text"
          value={newColor}
          onChange={(e) => setNewColor(e.target.value)}
          placeholder="Enter new color name"
          style={{ padding: "5px", marginRight: "10px" }}
        />
        <button onClick={handleAddColor} style={{ padding: "5px 10px" }}>
          Add Color
        </button>
      </div>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {colors.map((color) => (
            <tr key={color.id}>
              <td>{color.id}</td>
              <td>
                {editingId === color.id ? (
                  <input
                    type="text"
                    value={newColorName}
                    onChange={(e) => setNewColorName(e.target.value)}
                  />
                ) : (
                  color.name
                )}
              </td>
              <td>
                {editingId === color.id ? (
                  <button onClick={() => handleSaveClick(color.id)}>Save</button>
                ) : (
                  <button onClick={() => handleEditClick(color.id, color.name)}>
                    Edit
                  </button>
                )}
                <button onClick={() => handleDeleteClick(color.id)}>Delete</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      {isPopupOpen && (
        <ConfirmPopup
          message="Are you sure you want to delete this color?"
          onConfirm={handleConfirmDelete}
          onCancel={handleCancelDelete}
        />
      )}
    </div>
  );
};

export default ManageColors;
