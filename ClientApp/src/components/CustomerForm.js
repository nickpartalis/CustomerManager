import { useFormik } from 'formik';
import * as Yup from 'yup';
import { Button, Col, Container, Row } from 'react-bootstrap';

const CustomerForm = ({ initialCustomer, onFormSubmit, closeModal }) => {
  const validationSchema = Yup.object({
      firstName: Yup.string()
        .required('First name is required')
        .matches(/^[a-zA-Zα-ωΑ-Ωά-ώΆ-Ώ]+$/, 'Only letters are allowed')
        .min(2, 'First name must be at least 2 characters')
        .max(100, 'First name must be at most 100 characters'),
      lastName: Yup.string()
        .required('Last name is required')
        .matches(/^[a-zA-Zα-ωΑ-Ωά-ώΆ-Ώ\s]+$/, 'Only letters are allowed')
        .min(2, 'Last name must be at least 2 characters')
        .max(100, 'Last name must be at most 100 characters'),
      homeNumber: Yup.string(),
      workNumber: Yup.string(),
      mobileNumber: Yup.string()
        .test('at-least-one-number', 'At least one of the number fields should not be empty', function (values) {
          const { homeNumber, workNumber, mobileNumber } = this.parent;
          return Boolean(homeNumber || workNumber || mobileNumber);
      }),
      address: Yup.string()
        .required('Address is required')
        .max(150, 'Address must be at most 150 characters'),
      email: Yup.string().email('Invalid email address').required('Email is required'),
    })

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
    validationSchema,
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
            <label>First name:</label>
            <input
              type="text"
              name="firstName"
              value={formik.values.firstName}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              className={formik.touched.firstName && formik.errors.firstName ? 'error-input' : ''}
            />
            {formik.touched.firstName && formik.errors.firstName && (
              <div className="error">{formik.errors.firstName}</div>
            )}
          </Row>

          <Row>
            <label>Last name:</label>
            <input
              type="text"
              name="lastName"
              value={formik.values.lastName}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              className={formik.touched.lastName && formik.errors.lastName ? 'error-input' : ''}
            />
            {formik.touched.lastName && formik.errors.lastName && (
              <div className="error">{formik.errors.lastName}</div>
            )}
          </Row>

          <Row>
            <label>Home number:</label>
            <input
              type="text"
              name="homeNumber"
              value={formik.values.homeNumber}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              className={formik.touched.mobileNumber && formik.errors.mobileNumber ? 'error-input' : ''}
            />
          </Row>
          
          <Row>
            <label>Work number:</label>
            <input
              type="text"
              name="workNumber"
              value={formik.values.workNumber}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              className={formik.touched.mobileNumber && formik.errors.mobileNumber ? 'error-input' : ''}
            />
          </Row>

          <Row>
            <label>Mobile number:</label>
            <input
              type="text"
              name="mobileNumber"
              value={formik.values.mobileNumber}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              className={formik.touched.mobileNumber && formik.errors.mobileNumber ? 'error-input' : ''}
            />
            {formik.touched.mobileNumber && formik.errors.mobileNumber && (
              <div className="error">{formik.errors.mobileNumber}</div>
            )}
          </Row>

          <Row>
            <label>Address:</label>
            <input
              type="text"
              name="address"
              value={formik.values.address}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              className={formik.touched.address && formik.errors.address ? 'error-input' : ''}
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
              className={formik.touched.email && formik.errors.email ? 'error-input' : ''}
            />
            {formik.touched.email && formik.errors.email && (
              <div className="error">{formik.errors.email}</div>
            )}
          </Row>
          <div className="d-flex justify-content-end mt-4">
            <Button variant="primary" className="me-1" type="submit">{initialCustomer ? 'Update Customer' : 'Add Customer'}</Button>
            <Button variant="secondary" onClick={closeModal}>Close</Button>
          </div>
        </Col>
      </form>
    </Container>
  );
};

export default CustomerForm;