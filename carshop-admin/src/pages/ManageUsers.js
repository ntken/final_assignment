import { Link } from "react-router-dom";
import { useEffect, useState } from "react";
import axios from "axios";
import ConfirmPopup from "../components/ConfirmPopup";
import "../styles.css";

const ManageUsers = () => {
  const [users, setUsers] = useState([]);
  const [isPopupOpen, setIsPopupOpen] = useState(false);
  const [selectedUserId, setSelectedUserId] = useState(null);

  useEffect(() => {
    const fetchUsers = async () => {
      const token = localStorage.getItem("token");
      try {
        const response = await axios.get("http://localhost:5237/users", {
          headers: { Authorization: `Bearer ${token}` },
        });
        setUsers(response.data);
      } catch (error) {
        console.error("Error fetching users:", error);
      }
    };

    fetchUsers();
  }, []);

  const handleDeleteClick = (userId) => {
    setSelectedUserId(userId);
    setIsPopupOpen(true);
  };

  const handleConfirmDelete = async () => {
    const token = localStorage.getItem("token");
    try {
      await axios.delete(`http://localhost:5237/users/${selectedUserId}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      // Cập nhật danh sách users sau khi xóa
      setUsers((prevUsers) => prevUsers.filter((user) => user.id !== selectedUserId));
    } catch (error) {
      console.error("Error deleting user:", error);
    }
    setIsPopupOpen(false); // Đóng pop-up sau khi xóa
  };

  const handleCancelDelete = () => {
    setIsPopupOpen(false);
  };

  return (
    <div>
      <div>
        <Link to="/" className="back-to-dashboard">
          ← Back to Dashboard
        </Link>
      </div>
      <h2>Manage Users</h2>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Email</th>
            <th>Full Name</th>
            <th>Role</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {users.map((user) => (
            <tr key={user.id}>
              <td>{user.id}</td>
              <td>{user.email}</td>
              <td>{user.fullName}</td>
              <td>{user.role}</td>
              <td>
                {user.role !== "Admin" && ( // Chỉ hiển thị nút nếu Role khác Admin
                  <>                    
                    <button onClick={() => handleDeleteClick(user.id)}>Delete</button>
                  </>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {isPopupOpen && (
        <ConfirmPopup
          message="Are you sure you want to delete this user?"
          onConfirm={handleConfirmDelete}
          onCancel={handleCancelDelete}
        />
      )}
    </div>
  );
};

export default ManageUsers;
