import { useContext } from "react";
import { AuthContext } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";
import { Link } from "react-router-dom";

const Dashboard = () => {
  return (
    <div>
      <h1>Admin Dashboard</h1>
      <nav>
        <ul>
          <li><Link to="/manage-cars">Manage Cars</Link></li>
          <li><Link to="/manage-companies">Manage Companies</Link></li>
          <li><Link to="/manage-models">Manage Models</Link></li>
          <li><Link to="/manage-colors">Manage Colors</Link></li>
          <li><Link to="/manage-users">Manage Users</Link></li>
        </ul>
      </nav>
    </div>
  );
};

export default Dashboard;