import axios from 'axios';

const baseURL = 'https://localhost:7118/api/v1/customers';

const api = axios.create({ baseURL });

const getCustomers = async () => {
  const response = await api.get('/');
  return response.data;
};

const getCustomer = async (id) => {
  const response = await api.get(`/${id}`);
  return response.data;
};

const createCustomer = async (customer) => {
  const response = await api.post('/', customer);
  return response.data;
};

const updateCustomer = async (id, customer) => {
  const response = await api.put(`/${id}`, customer);
  return response.data;
};

const deleteCustomer = async (id) => {
  await api.delete(`/${id}`);
};

// eslint-disable-next-line import/no-anonymous-default-export
export default { getCustomers, getCustomer, createCustomer, updateCustomer, deleteCustomer };
