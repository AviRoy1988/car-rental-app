import React, { useState } from "react";
import PickupForm from "../components/PickupForm";
import ReturnForm from "../components/ReturnForm";
import RentalList from "../components/RentalList";
import "./HomePage.css";

type Tab = "pickup" | "return" | "all" | "active";

const HomePage: React.FC = () => {
  const [activeTab, setActiveTab] = useState<Tab>("pickup");

  return (
    <div className="home-page">
      <header className="app-header">
        <h1>🚗 Car Rental Management System</h1>
        <p>Manage vehicle pickups, returns, and rentals</p>
      </header>

      <nav className="tab-navigation">
        <button
          className={activeTab === "pickup" ? "tab active" : "tab"}
          onClick={() => setActiveTab("pickup")}
        >
          Register Pickup
        </button>
        <button
          className={activeTab === "return" ? "tab active" : "tab"}
          onClick={() => setActiveTab("return")}
        >
          Register Return
        </button>
        <button
          className={activeTab === "active" ? "tab active" : "tab"}
          onClick={() => setActiveTab("active")}
        >
          Active Rentals
        </button>
        <button
          className={activeTab === "all" ? "tab active" : "tab"}
          onClick={() => setActiveTab("all")}
        >
          All Rentals
        </button>
      </nav>

      <main className="page-content">
        {activeTab === "pickup" && <PickupForm />}
        {activeTab === "return" && <ReturnForm />}
        {activeTab === "active" && <RentalList activeOnly={true} />}
        {activeTab === "all" && <RentalList activeOnly={false} />}
      </main>

      <footer className="app-footer">
        <p>Car Rental Service &copy; 2026</p>
      </footer>
    </div>
  );
};

export default HomePage;
