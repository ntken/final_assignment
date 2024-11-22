import { useEffect, useState } from "react";
import axios from "axios";
import { Link } from "react-router-dom";
import ConfirmPopup from "../components/ConfirmPopup";
import "../styles.css";

const ManageCars = () => {
  const [cars, setCars] = useState([]);
  const [isPopupOpen, setIsPopupOpen] = useState(false);
  const [selectedCarId, setSelectedCarId] = useState(null);

  useEffect(() => {
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

    fetchCars();
  }, []);

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

  return (
    <div>
      <div>
        <Link to="/" className="back-to-dashboard">
          ← Back to Dashboard
        </Link>
      </div>
      <h2>Manage Cars</h2>
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
                  src={`http://localhost:5216${car.image}`} // Đường dẫn đầy đủ tới ảnh
                  alt={`${car.company} ${car.model}`} // Văn bản thay thế
                  style={{ width: "100px", height: "auto" }} // Kích thước ảnh
                  onError={(e) => (e.target.src = "/placeholder-image.png")} // Ảnh mặc định khi lỗi
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
                <button onClick={() => alert(`Edit car ${car.company} ${car.model}`)}>Edit</button>
                <button onClick={() => handleDeleteClick(car.id)}>Delete</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      {isPopupOpen && (
        <ConfirmPopup
          message="Are you sure you want to delete this item?"
          onConfirm={handleConfirmDelete}
          onCancel={handleCancelDelete}
        />
      )}
    </div>
  );
};

export default ManageCars;
