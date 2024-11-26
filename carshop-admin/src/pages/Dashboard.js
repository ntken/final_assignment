import { Link } from "react-router-dom";
import "../styles.css";

const Dashboard = () => {
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
