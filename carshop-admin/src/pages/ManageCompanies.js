import { useEffect, useState } from "react";
import axios from "axios";

const ManageCompanies = () => {
  const [companies, setCompanies] = useState([]);
  const [newCompany, setNewCompany] = useState("");

  useEffect(() => {
    fetchCompanies();
  }, []);

  const fetchCompanies = async () => {
    const token = localStorage.getItem("token");
    const response = await axios.get("http://localhost:5237/companies", {
      headers: { Authorization: `Bearer ${token}` },
    });
    setCompanies(response.data);
  };

  const handleAddCompany = async () => {
    const token = localStorage.getItem("token");
    await axios.post(
      "http://localhost:5237/companies",
      { name: newCompany },
      { headers: { Authorization: `Bearer ${token}` } }
    );
    setNewCompany("");
    fetchCompanies();
  };

  const handleDeleteCompany = async (id) => {
    const token = localStorage.getItem("token");
    await axios.delete(`http://localhost:5237/companies/${id}`, {
      headers: { Authorization: `Bearer ${token}` },
    });
    fetchCompanies();
  };

  return (
    <div>
      <h2>Manage Companies</h2>
      <div>
        <input
          type="text"
          value={newCompany}
          onChange={(e) => setNewCompany(e.target.value)}
          placeholder="New Company"
        />
        <button onClick={handleAddCompany}>Add Company</button>
      </div>
      <ul>
        {companies.map((company) => (
          <li key={company.id}>
            {company.name}{" "}
            <button onClick={() => handleDeleteCompany(company.id)}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default ManageCompanies;
