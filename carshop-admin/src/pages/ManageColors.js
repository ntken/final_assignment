import { useEffect, useState } from "react";
import axios from "axios";

const ManageColors = () => {
  const [colors, setColors] = useState([]);
  const [newColor, setNewColor] = useState("");

  useEffect(() => {
    fetchColors();
  }, []);

  const fetchColors = async () => {
    const token = localStorage.getItem("token");
    const response = await axios.get("http://localhost:5237/colors", {
      headers: { Authorization: `Bearer ${token}` },
    });
    setColors(response.data);
  };

  const handleAddColor = async () => {
    const token = localStorage.getItem("token");
    await axios.post(
      "http://localhost:5237/colors",
      { name: newColor },
      { headers: { Authorization: `Bearer ${token}` } }
    );
    setNewColor("");
    fetchColors();
  };

  const handleDeleteColor = async (id) => {
    const token = localStorage.getItem("token");
    await axios.delete(`http://localhost:5237/colors/${id}`, {
      headers: { Authorization: `Bearer ${token}` },
    });
    fetchColors();
  };

  return (
    <div>
      <h2>Manage Colors</h2>
      <div>
        <input
          type="text"
          value={newColor}
          onChange={(e) => setNewColor(e.target.value)}
          placeholder="New Color"
        />
        <button onClick={handleAddColor}>Add Color</button>
      </div>
      <ul>
        {colors.map((color) => (
          <li key={color.id}>
            {color.name}{" "}
            <button onClick={() => handleDeleteColor(color.id)}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default ManageColors;
