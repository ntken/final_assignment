import { Link, useNavigate } from "react-router-dom";
import { useEffect } from "react";
import "../styles.css";

const Dashboard = () => {
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (!token) {
      navigate("/login"); // Nếu không có token, điều hướng tới Login
      return;
    }

    const decodedToken = JSON.parse(atob(token.split(".")[1])); // Decode JWT payload
    if (decodedToken["Role"] !== "Admin") {
      alert("Access denied. Only Admin users can access this page.");
      localStorage.removeItem("token"); // Xóa token nếu không đủ quyền
      navigate("/login");
    }
  }, [navigate]);

  return (
    <div className="dashboard-container">
      <h1>Admin Dashboard</h1>
      <div className="dashboard-links">
        <Link to="/manage-cars" className="dashboard-link">
          Manage Cars
        </Link>
        <Link to="/manage-companies" className="dashboard-link">
          Manage Companies
        </Link>
        <Link to="/manage-models" className="dashboard-link">
          Manage Models
        </Link>
        <Link to="/manage-colors" className="dashboard-link">
          Manage Colors
        </Link>
        <Link to="/manage-users" className="dashboard-link">
          Manage Users
        </Link>
      </div>
    </div>
  );
};

export default Dashboard;
