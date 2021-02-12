import React, { useEffect, useState } from "react";
import { Container } from "@material-ui/core";
import CreateTravelPlan from "../../components/CreateTravelPlan";
import { useAuth } from "../../contexts/AuthContext";
import {
  createTravelPlan,
  getAvailableCars,
  getEmployees,
  getLocations,
} from "../../axios/carpoolAxios";
import { useFormik } from "formik";

const Carpool = (props) => {
  const { userLoaded } = useAuth();

  const [employeeData, setEmployeeData] = useState([]);
  const [availableCars, setAvailableCars] = useState([]);
  const [locations, setLocations] = useState([]);
  const [errorMessage, setErrorMessage] = useState("");

  const today = new Date();
  const tomorrow = new Date(today);
  tomorrow.setDate(tomorrow.getDate() + 1);

  const formik = useFormik({
    initialValues: {
      startDateUtc: today,
      endDateUtc: tomorrow,
      employeeIds: [],
      startLocationId: 0,
      endLocationId: 0,
      carId: 0,
    },
    onSubmit: (values) => {
      setErrorMessage("");
      createTravelPlan(values)
        .then((response) => {
          alert("Success");
        })
        .catch((e) => {
          setErrorMessage(e.response.data);
        });
    },
  });

  function fetchAvailableCars() {
    const travelPlanOptions = {
      startDateUtc: formik.values.startDateUtc,
      endDateUtc: formik.values.endDateUtc,
      startLocationId: formik.values.startLocationId,
      endLocationId: formik.values.endLocationId,
    };
    getAvailableCars(travelPlanOptions).then(({ data }) => {
      setAvailableCars(data.cars);
    });
  }

  function fetchEmployees() {
    getEmployees(formik.values.startDateUtc, formik.values.endDateUtc).then(
      ({ data }) => {
        setEmployeeData(data);
      }
    );
  }

  useEffect(() => {
    if (userLoaded) {
      fetchEmployees();
    }
  }, [userLoaded]);

  useEffect(() => {
    if (userLoaded) {
      getLocations().then(({ data }) => {
        setLocations(data.locations);
      });
    }
  }, [userLoaded]);

  useEffect(() => {
    if (userLoaded) {
      fetchAvailableCars();
    }
  }, [userLoaded]);
  return (
    <Container>
      <CreateTravelPlan
        employeeData={employeeData}
        formik={formik}
        cars={availableCars}
        fetchCars={fetchAvailableCars}
        locations={locations}
        errorMessage={errorMessage}
        fetchEmployees={fetchEmployees}
      />
    </Container>
  );
};

export default Carpool;
