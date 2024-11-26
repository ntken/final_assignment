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

  const handleCancelEdit = () => {
    setEditingCar(null); // Hủy bỏ chỉnh sửa
  };

  const handleInputChange = (field, value) => {
    setEditingCar({ ...editingCar, [field]: value });
  };

  return (
    <div>
      <div>
        <Link to="/" className="back-to-dashboard">
          ← Back to Dashboard
        </Link>
      </div>
      <h2>Manage Cars</h2>
      {editingCar ? (
        <div className="edit-form">
          <h3>Edit Car</h3>
          <div>
            <label>Car Name:</label>
            <input type="text" value={`${editingCar.company} ${editingCar.model}`} readOnly />
          </div>
          <div>
            <label>Company:</label>
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
            <label>Model:</label>
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
            <label>Color:</label>
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
            <label>Image URL:</label>
            <input
              type="text"
              value={editingCar.image}
              onChange={(e) => handleInputChange("image", e.target.value)}
            />
          </div>
          <div>
            <label>Price:</label>
            <input
              type="number"
              value={editingCar.price}
              onChange={(e) => handleInputChange("price", parseFloat(e.target.value))}
            />
          </div>
          <div>
            <label>Released Date:</label>
            <input
              type="date"
              value={editingCar.releasedDate.split("T")[0]}
              onChange={(e) => handleInputChange("releasedDate", e.target.value)}
            />
          </div>
          <div>
            <label>Description:</label>
            <textarea
              value={editingCar.description}
              onChange={(e) => handleInputChange("description", e.target.value)}
            ></textarea>
          </div>
          <button onClick={handleSaveClick}>Save</button>
          <button onClick={handleCancelEdit}>Cancel</button>
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
                  <button onClick={() => handleEditClick(car)}>Edit</button>
                  <button onClick={() => handleDeleteClick(car.id)}>Delete</button>
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
    </div>
  );
};

export default ManageCars;