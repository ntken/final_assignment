import { useEffect, useState } from "react";
import axios from "axios";
import { Link } from "react-router-dom";
import ConfirmPopup from "../components/ConfirmPopup";
import "../styles.css";

const ManageCars = () => {
  const [cars, setCars] = useState([]);
  const [companies, setCompanies] = useState([]);
  const [models, setModels] = useState([]);
  const [colors, setColors] = useState([]);
  const [isPopupOpen, setIsPopupOpen] = useState(false);
  const [selectedCarId, setSelectedCarId] = useState(null);
  const [editingCar, setEditingCar] = useState(null);
  const [isAddPopupOpen, setIsAddPopupOpen] = useState(false);
  const [newCar, setNewCar] = useState({
    companyId: "",
    modelId: "",
    colorId: "",
    price: "",
    releasedDate: "",
    description: "",
    image: "",
  });

  useEffect(() => {
    fetchCars();
    fetchCompanies();
    fetchModels();
    fetchColors();
  }, []);

  const fetchCars = async () => {
    const token = localStorage.getItem("token");
    try {
      const response = await axios.get("http://localhost:5237/cars", {
        headers: { Authorization: `Bearer ${token}` },
      });
      setCars(response.data);
    } catch (error) {
      console.error("Error fetching cars:", error);
    }
  };

  const fetchCompanies = async () => {
    const token = localStorage.getItem("token");
    try {
      const response = await axios.get("http://localhost:5237/companies", {
        headers: { Authorization: `Bearer ${token}` },
      });
      setCompanies(response.data);
    } catch (error) {
      console.error("Error fetching companies:", error);
    }
  };

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

  const handleDeleteClick = (carId) => {
    setSelectedCarId(carId);
    setIsPopupOpen(true);
  };

  const handleConfirmDelete = async () => {
    const token = localStorage.getItem("token");
    try {
      await axios.delete(`http://localhost:5237/cars/${selectedCarId}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      // Cập nhật danh sách cars sau khi xóa
      setCars((prevCars) => prevCars.filter((car) => car.id !== selectedCarId));
    } catch (error) {
      console.error("Error deleting car:", error);
    }
    setIsPopupOpen(false); // Đóng pop-up sau khi xóa
  };

  const handleCancelDelete = () => {
    setIsPopupOpen(false);
    setSelectedCarId(null);
  };

  const handleEditClick = (car) => {
    setEditingCar({ ...car }); // Clone car để chỉnh sửa
  };

  const handleSaveClick = async () => {
    const token = localStorage.getItem("token");
    try {
      await axios.put(
        `http://localhost:5237/cars/${editingCar.id}`,
        {
          companyId: editingCar.companyId,
          modelId: editingCar.modelId,
          colorId: editingCar.colorId,
          price: editingCar.price,
          releasedDate: editingCar.releasedDate,
          description: editingCar.description,
          image: editingCar.image,
        },
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      setEditingCar(null);
      fetchCars(); // Reload danh sách xe sau khi lưu
    } catch (error) {
      console.error("Error updating car:", error);
    }
  };

  const handleAddCar = async () => {
    const token = localStorage.getItem("token");
    try {
      await axios.post(
        "http://localhost:5237/cars",
        {
          ...newCar,
          price: parseFloat(newCar.price),
        },
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      setIsAddPopupOpen(false);
      setNewCar({
        companyId: "",
        modelId: "",
        colorId: "",
        price: "",
        releasedDate: "",
        description: "",
        image: "",
      });
      fetchCars();
    } catch (error) {
      console.error("Error adding car:", error);
    }
  };

  const handleCancelEdit = () => {
    setEditingCar(null); // Hủy bỏ chỉnh sửa
  };

  const handleInputChange = (field, value, isAddMode = false) => {
    if (isAddMode) {
      setNewCar({ ...newCar, [field]: value });
    } else {
      setEditingCar({ ...editingCar, [field]: value });
    }
  };

  return (
    <div>
      <div>
        <Link to="/" className="back-to-dashboard">
          ← Back to Dashboard
        </Link>
      </div>
      <h2>Manage Cars</h2>
      <button
        className="add-btn"
        onClick={() => setIsAddPopupOpen(true)}
        style={{ marginBottom: "20px" }}
      >
        Add New Car
      </button>
      {editingCar ? (
        <div className="edit-form">
          <h3>Edit Car</h3>
          <div>
            <label>Car Name: </label>
            <input type="text" value={`${editingCar.company} ${editingCar.model}`} readOnly />
          </div>
          <div>
            <label>Company: </label>
            <select
              value={editingCar.companyId}
              onChange={(e) => handleInputChange("companyId", parseInt(e.target.value))}
            >
              {companies.map((company) => (
                <option key={company.id} value={company.id}>
                  {company.name}
                </option>
              ))}
            </select>
          </div>
          <div>
            <label>Model: </label>
            <select
              value={editingCar.modelId}
              onChange={(e) => handleInputChange("modelId", parseInt(e.target.value))}
            >
              {models.map((model) => (
                <option key={model.id} value={model.id}>
                  {model.name}
                </option>
              ))}
            </select>
          </div>
          <div>
            <label>Color: </label>
            <select
              value={editingCar.colorId}
              onChange={(e) => handleInputChange("colorId", parseInt(e.target.value))}
            >
              {colors.map((color) => (
                <option key={color.id} value={color.id}>
                  {color.name}
                </option>
              ))}
            </select>
          </div>
          <div>
            <label>Image URL: </label>
            <input
              type="text"
              value={editingCar.image}
              onChange={(e) => handleInputChange("image", e.target.value)}
            />
          </div>
          <div>
            <label>Price: </label>
            <input
              type="number"
              value={editingCar.price}
              onChange={(e) => handleInputChange("price", parseFloat(e.target.value))}
            />
          </div>
          <div>
            <label>Released Date: </label>
            <input
              type="date"
              value={editingCar.releasedDate.split("T")[0]}
              onChange={(e) => handleInputChange("releasedDate", e.target.value)}
            />
          </div>
          <div>
            <label>Description: </label>
            <textarea
              rows="4"
              value={editingCar.description}
              onChange={(e) => handleInputChange("description", e.target.value)}
            ></textarea>
          </div>
          <div style={{ textAlign: "center" }}>
            <button onClick={handleSaveClick}>Save</button>
            <button onClick={handleCancelEdit}>Cancel</button>
          </div>
        </div>
      ) : (
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Image</th>
              <th>Car Name</th>
              <th>Company</th>
              <th>Model</th>
              <th>Color</th>
              <th>Price</th>
              <th>Released Date</th>
              <th>Description</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {cars.map((car) => (
              <tr key={car.id}>
                <td>{car.id}</td>
                <td>
                  <img
                    src={`http://localhost:5216${car.image}`}
                    alt={`${car.company} ${car.model}`}
                    style={{ width: "100px", height: "auto" }}
                    onError={(e) => (e.target.src = "/placeholder-image.png")}
                  />
                </td>
                <td>{`${car.company} ${car.model}`}</td>
                <td>{car.company}</td>
                <td>{car.model}</td>
                <td>{car.color}</td>
                <td>${car.price.toLocaleString()}</td>
                <td>{new Date(car.releasedDate).toLocaleDateString()}</td>
                <td>{car.description}</td>
                <td>
                  <button onClick={() => handleEditClick(car)} className="edit-btn">Edit</button>
                  <button onClick={() => handleDeleteClick(car.id)} className="delete-btn">Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
      {isPopupOpen && (
        <ConfirmPopup
          message="Are you sure you want to delete this car?"
          onConfirm={handleConfirmDelete}
          onCancel={handleCancelDelete}
        />
      )}
      {isAddPopupOpen && (
        <div className="modal-overlay">
          <div className="modal-content">
            <h3>Add New Car</h3>
            <div>
              <label>Company:</label>
              <select
                value={newCar.companyId}
                onChange={(e) => handleInputChange("companyId", e.target.value, true)}
              >
                <option value="">Select Company</option>
                {companies.map((company) => (
                  <option key={company.id} value={company.id}>
                    {company.name}
                  </option>
                ))}
              </select>
            </div>
            <div>
              <label>Model:</label>
              <select
                value={newCar.modelId}
                onChange={(e) => handleInputChange("modelId", e.target.value, true)}
              >
                <option value="">Select Model</option>
                {models.map((model) => (
                  <option key={model.id} value={model.id}>
                    {model.name}
                  </option>
                ))}
              </select>
            </div>
            <div>
              <label>Color:</label>
              <select
                value={newCar.colorId}
                onChange={(e) => handleInputChange("colorId", e.target.value, true)}
              >
                <option value="">Select Color</option>
                {colors.map((color) => (
                  <option key={color.id} value={color.id}>
                    {color.name}
                  </option>
                ))}
              </select>
            </div>
            <div>
              <label>Released Date:</label>
              <input
                type="date"
                value={newCar.releasedDate}
                onChange={(e) => handleInputChange("releasedDate", e.target.value, true)}
              />
            </div>
            <div>
              <label>Price:</label>
              <input
                type="number"
                value={newCar.price}
                onChange={(e) => handleInputChange("price", e.target.value, true)}
              />
            </div>
            <div>
              <label>Description:</label>
              <textarea
                rows="4"
                value={newCar.description}
                onChange={(e) => handleInputChange("description", e.target.value, true)}
              ></textarea>
            </div>
            <div>
              <label>Image URL:</label>
              <input
                type="text"
                value={newCar.image}
                placeholder="/images/<yourimage.jpg>"
                onChange={(e) => handleInputChange("image", e.target.value, true)}
              />
            </div>
            <div className="modal-buttons">
              <button className="btn-cancel" onClick={() => setIsAddPopupOpen(false)}>
                Cancel
              </button>
              <button className="btn-confirm" onClick={handleAddCar}>
                Submit
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default ManageCars;