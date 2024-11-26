import { useEffect, useState } from "react";
import axios from "axios";
import { Link } from "react-router-dom";
import ConfirmPopup from "../components/ConfirmPopup";
import "../styles.css";

const ManageCompanies = () => {
  const [companies, setCompanies] = useState([]);
  const [editingId, setEditingId] = useState(null);
  const [newCompanyName, setNewCompanyName] = useState("");
  const [isPopupOpen, setIsPopupOpen] = useState(false);
  const [selectedCompanyId, setSelectedCompanyId] = useState(null);
  const [newCompany, setNewCompany] = useState("");

  useEffect(() => {
    fetchCompanies();
  }, []);

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

  const handleAddCompany = async () => {
    const token = localStorage.getItem("token");
    if (!newCompany.trim()) {
      alert("Company name cannot be empty!");
      return;
    }
    try {
      await axios.post(
        "http://localhost:5237/companies",
        { name: newCompany },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setNewCompany("");
      fetchCompanies();
    } catch (error) {
      console.error("Error adding company:", error);
    }
  };

  const handleDeleteClick = (companyId) => {
    setSelectedCompanyId(companyId);
    setIsPopupOpen(true);
  };

  const handleConfirmDelete = async () => {
    const token = localStorage.getItem("token");
    try {
      await axios.delete(`http://localhost:5237/companies/${selectedCompanyId}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      fetchCompanies();
    } catch (error) {
      console.error("Error deleting company:", error);
    }
    setIsPopupOpen(false);
  };

  const handleCancelDelete = () => {
    setIsPopupOpen(false);
    setSelectedCompanyId(null);
  };

  const handleEditClick = (id, currentName) => {
    setEditingId(id);
    setNewCompanyName(currentName);
  };

  const handleSaveClick = async (id) => {
    const token = localStorage.getItem("token");
    try {
      await axios.put(
        `http://localhost:5237/companies/${id}`,
        { name: newCompanyName },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setEditingId(null);
      fetchCompanies();
    } catch (error) {
      console.error("Error updating company:", error);
    }
  };

  return (
    <div>
      <Link to="/" className="back-to-dashboard">← Back to Dashboard</Link>
      <h2>Manage Companies</h2>
      <div className="form-container">
        <input
          type="text"
          value={newCompany}
          onChange={(e) => setNewCompany(e.target.value)}
          placeholder="Enter new company name"
        />
        <button onClick={handleAddCompany} className="add-btn">Add Company</button>
      </div>
      <div className="table-wrapper">
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Name</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {companies.map((company) => (
              <tr key={company.id}>
                <td>{company.id}</td>
                <td>
                  {editingId === company.id ? (
                    <input
                      type="text"
                      value={newCompanyName}
                      onChange={(e) => setNewCompanyName(e.target.value)}
                    />
                  ) : (
                    company.name
                  )}
                </td>
                <td>
                  {editingId === company.id ? (
                    <button onClick={() => handleSaveClick(company.id)} className="edit-btn">Save</button>
                  ) : (
                    <button onClick={() => handleEditClick(company.id, company.name)} className="edit-btn">
                      Edit
                    </button>
                  )}
                  <button onClick={() => handleDeleteClick(company.id)} className="delete-btn">Delete</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      {isPopupOpen && (
        <ConfirmPopup
          message="Are you sure you want to delete this company?"
          onConfirm={handleConfirmDelete}
          onCancel={handleCancelDelete}
        />
      )}

    </div>
  );
};

export default ManageCompanies;
