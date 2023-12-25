import { useFormik } from 'formik';
import * as Yup from 'yup';
import { Button, Col, Container, Row } from 'react-bootstrap';

const CustomerForm = ({ initialCustomer, onFormSubmit, closeModal }) => {
  const formik = useFormik({
    initialValues: {
      firstName: '',
      lastName: '',
      homeNumber: '',
      workNumber: '',
      mobileNumber: '',
      address: '',
      email: '',
      ...initialCustomer,
    },
    validationSchema: Yup.object({
      firstName: Yup.string()
        .required('First name is required')
        .matches(/^[a-zA-Zα-ωΑ-Ωά-ώΆ-Ώ]+$/, 'Only letters are allowed')
        .min(2, 'First name must be at least 2 characters')
        .max(100, 'First name must be at most 100 characters'),
      lastName: Yup.string()
        .required('Last name is required')
        .matches(/^[a-zA-Zα-ωΑ-Ωά-ώΆ-Ώ]+$/, 'Only letters are allowed')
        .min(2, 'Last name must be at least 2 characters')
        .max(100, 'Last name must be at most 100 characters'),
      homeNumber: Yup.string(),
      workNumber: Yup.string(),
      mobileNumber: Yup.string(),
      address: Yup.string()
        .required('Address is required')
        .max(150, 'Address must be at most 150 characters'),
      email: Yup.string().email('Invalid email address').required('Email is required'),
    })
    .test('at-least-one-number', 'At least one of the number fields should not be empty', function (values) {
      const { homeNumber, workNumber, mobileNumber } = values;
      return homeNumber || workNumber || mobileNumber;
    }),
    onSubmit: async (values) => {
      onFormSubmit(values);
      formik.resetForm();
    },
  });

  return (
    <Container>
      <form onSubmit={formik.handleSubmit}>
        <Col>
          <Row>
            <label>First Name:</label>
            <input
              type="text"
              name="firstName"
              value={formik.values.firstName}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
            />
            {formik.touched.firstName && formik.errors.firstName && (
              <div className="error">{formik.errors.firstName}</div>
            )}
          </Row>

          <Row>
            <label>Last Name:</label>
            <input
              type="text"
              name="lastName"
              value={formik.values.lastName}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
            />
            {formik.touched.lastName && formik.errors.lastName && (
              <div className="error">{formik.errors.lastName}</div>
            )}
          </Row>
          {/* TODO: Validation for numbers */}
          <Row>
            <label>Home Number:</label>
            <input
              type="text"
              name="homeNumber"
              value={formik.values.homeNumber}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
            />
          </Row>
          
          <Row>
            <label>Work Number:</label>
            <input
              type="text"
              name="workNumber"
              value={formik.values.workNumber}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
            />
          </Row>

          <Row>
            <label>Mobile Number:</label>
            <input
              type="text"
              name="mobileNumber"
              value={formik.values.mobileNumber}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
            />
          </Row>

          <Row>
            <label>Address:</label>
            <input
              type="text"
              name="address"
              value={formik.values.address}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
            />
            {formik.touched.address && formik.errors.address && (
              <div className="error">{formik.errors.address}</div>
            )}
          </Row>

          <Row>
            <label>Email:</label>
            <input
              type="text"
              name="email"
              value={formik.values.email}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
            />
            {formik.touched.email && formik.errors.email && (
              <div className="error">{formik.errors.email}</div>
            )}
          </Row>

          <Button variant="primary" type="submit">{initialCustomer ? 'Update Customer' : 'Add Customer'}</Button>
          <Button variant="secondary" onClick={closeModal}>Close</Button>
        </Col>
      </form>
    </Container>
  );
};

export default CustomerForm;