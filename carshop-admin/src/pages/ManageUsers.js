import { useEffect, useState } from "react";
import axios from "axios";

const ManageUsers = () => {
  const [users, setUsers] = useState([]);

  useEffect(() => {
    fetchUsers();
  }, []);

  const fetchUsers = async () => {
    const token = localStorage.getItem("token");
    const response = await axios.get("http://localhost:5237/users/admin/users", {
      headers: { Authorization: `Bearer ${token}` },
    });
    setUsers(response.data);
  };

  const handleDeleteUser = async (id) => {
    const token = localStorage.getItem("token");
    await axios.delete(`http://localhost:5237/users/${id}`, {
      headers: { Authorization: `Bearer ${token}` },
    });
    fetchUsers();
  };

  return (
    <div>
      <h2>Manage Users</h2>
      <ul>
        {users.map((user) => (
          <li key={user.id}>
            {user.fullName} ({user.email}){" "}
            <button onClick={() => handleDeleteUser(user.id)}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default ManageUsers;
