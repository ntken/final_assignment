import { useEffect, useState } from "react";
import axios from "axios";

const ManageModels = () => {
  const [models, setModels] = useState([]);
  const [newModel, setNewModel] = useState("");

  useEffect(() => {
    fetchModels();
  }, []);

  const fetchModels = async () => {
    const token = localStorage.getItem("token");
    const response = await axios.get("http://localhost:5237/models", {
      headers: { Authorization: `Bearer ${token}` },
    });
    setModels(response.data);
  };

  const handleAddModel = async () => {
    const token = localStorage.getItem("token");
    await axios.post(
      "http://localhost:5237/models",
      { name: newModel },
      { headers: { Authorization: `Bearer ${token}` } }
    );
    setNewModel("");
    fetchModels();
  };

  const handleDeleteModel = async (id) => {
    const token = localStorage.getItem("token");
    await axios.delete(`http://localhost:5237/models/${id}`, {
      headers: { Authorization: `Bearer ${token}` },
    });
    fetchModels();
  };

  return (
    <div>
      <h2>Manage Models</h2>
      <div>
        <input
          type="text"
          value={newModel}
          onChange={(e) => setNewModel(e.target.value)}
          placeholder="New Model"
        />
        <button onClick={handleAddModel}>Add Model</button>
      </div>
      <ul>
        {models.map((model) => (
          <li key={model.id}>
            {model.name}{" "}
            <button onClick={() => handleDeleteModel(model.id)}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default ManageModels;
