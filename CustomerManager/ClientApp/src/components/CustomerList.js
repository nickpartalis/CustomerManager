import { useState, useEffect, useMemo } from 'react';
import { useTable } from 'react-table';
import { Modal, Button } from 'react-bootstrap';
import customerService from '../services/customerService';
import CustomerForm from './CustomerForm';
import 'bootstrap/dist/css/bootstrap.min.css';

const CustomerList = () => {
  const [customers, setCustomers] = useState([]);
  const [selectedCustomer, setSelectedCustomer] = useState(null);
  const [isFormModalOpen, setIsFormModalOpen] = useState(false);
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);

  useEffect(() => {
    const fetchCustomers = async () => {
      const data = await customerService.getCustomers();
      setCustomers(data);
    };

    fetchCustomers();
  }, []);

  const columns = useMemo(
    () => [
      {
        Header: '#',
        accessor: (row, index) => index + 1,
      },
      {
        Header: 'First Name',
        accessor: 'firstName',
      },
      {
        Header: 'Last Name',
        accessor: 'lastName',
      },
      {
        Header: 'Phone',
        accessor: (row) =>
          [row.homeNumber, row.workNumber, row.mobileNumber]
            .filter((number) => number)
            .join(' '),
      },
      {
        Header: 'Address',
        accessor: 'address',
      },
      {
        Header: 'Email',
        accessor: 'email',
      },
      {
        Header: '',
        accessor: 'id',
        Cell: ({ row }) => (
          <div>
            <Button variant="primary" onClick={() => openFormModal(row.original)}>Edit</Button>
            <Button variant="danger" onClick={() => handleDelete(row.original.id)}>Delete</Button>
          </div>
        ),
      },
    ],
    []
  );

  const { getTableProps, getTableBodyProps, headerGroups, rows, prepareRow } = useTable({ columns, data: customers });

  const openFormModal = (customer) => {
    setSelectedCustomer(customer);
    setIsFormModalOpen(true);
  };

  const closeFormModal = () => {
    setSelectedCustomer(null);
    setIsFormModalOpen(false);
  };

  const handleDelete = async (id) => {
    setSelectedCustomer(id);
    setIsDeleteModalOpen(true);
  };

  const confirmDelete = async () => {
    await customerService.deleteCustomer(selectedCustomer);
    setCustomers(customers.filter((customer) => customer.id !== selectedCustomer));
    setIsDeleteModalOpen(false);
    setSelectedCustomer(null);
  };

  const handleFormSubmit = async (values) => {
    if (selectedCustomer) {
      // Edit customer
      const id = selectedCustomer.id;
      await customerService.updateCustomer(id, {id, ...values });
      setCustomers((prevCustomers) =>
        prevCustomers.map((customer) => (customer.id === selectedCustomer.id ? { ...customer, ...values } : customer))
      );
    } else {
      // Add customer
      const newCustomer = await customerService.createCustomer(values);
      setCustomers([...customers, newCustomer]);
    }

    closeFormModal();
  };

  return (
    <div>
      <h2>Customer List</h2>
      <Button variant="primary" onClick={() => openFormModal()}>Add Customer</Button>
      <table {...getTableProps()} style={{ width: '100%' }}>
        <thead>
          {headerGroups.map((headerGroup) => (
            <tr {...headerGroup.getHeaderGroupProps()}>
              {headerGroup.headers.map((column) => (
                <th {...column.getHeaderProps()}>{column.render('Header')}</th>
              ))}
            </tr>
          ))}
        </thead>
        
        <tbody {...getTableBodyProps()}>
          {rows.map((row) => {
            prepareRow(row);
            return (
              <tr {...row.getRowProps()}>
                {row.cells.map((cell) => (
                  <td {...cell.getCellProps()}>{cell.render('Cell')}</td>
                ))}
              </tr>
            );
          })}
        </tbody>
      </table>

      {/* <Modal isOpen={isModalOpen} onRequestClose={closeModal}>
        {selectedCustomer && (
          <CustomerDetails 
            customer={selectedCustomer}
            handleClose={closeModal}
            handleEdit={handleEditClick}
            handleDelete={handleDelete}
          />
        )}
      </Modal> */}

      <Modal show={isFormModalOpen} onHide={closeFormModal}>
        <Modal.Header closeButton>
          <Modal.Title>{selectedCustomer ? 'Edit Customer' : 'Add Customer'}</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <CustomerForm 
            initialCustomer={selectedCustomer} 
            onFormSubmit={handleFormSubmit} 
            closeModal={closeFormModal}
          />
        </Modal.Body>
      </Modal>

      <Modal show={isDeleteModalOpen} onHide={() => setIsDeleteModalOpen(false)}>
        <Modal.Header closeButton>
          <Modal.Title>Confirm Delete</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          Are you sure you want to delete this customer?
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setIsDeleteModalOpen(false)}>
            Cancel
          </Button>
          <Button variant="danger" onClick={confirmDelete}>
            Delete
          </Button>
        </Modal.Footer>
      </Modal>

    </div>
  );
};

export default CustomerList;