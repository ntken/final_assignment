import { useEffect, useState } from "react";
import axios from "axios";

const ManageCars = () => {
  const [cars, setCars] = useState([]);

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

  return (
    <div>
      <h2>Manage Cars</h2>
      <table border="1" cellPadding="10" style={{ width: "100%", textAlign: "left" }}>
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
                <button onClick={() => alert(`Delete car ${car.company} ${car.model}`)}>Delete</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ManageCars;
