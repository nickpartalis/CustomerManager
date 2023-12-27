import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import CustomerList from './components/CustomerList';
// import CustomerForm from './components/CustomerForm';
import './App.css';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<CustomerList />} />
        {/* <Route path="/customer/new" element={<CustomerForm />} /> */}
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </Router>
  );
}

export default App;
