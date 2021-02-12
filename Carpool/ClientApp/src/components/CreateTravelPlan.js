import React, { useState } from "react";
import { DateTimePicker } from "@material-ui/pickers";
import Grid from "@material-ui/core/Grid";
import {
  Button,
  Container,
  MenuItem,
  Select,
  TextField,
} from "@material-ui/core";
import Autocomplete from "@material-ui/lab/Autocomplete";
import Typography from "@material-ui/core/Typography";
import Box from "@material-ui/core/Box";

const CreateTravelPlan = ({
  employeeData,
  formik,
  cars,
  fetchCars,
  locations,
  errorMessage,
  fetchEmployees,
}) => {
  const [minDate, setMinDate] = useState(formik.values.endDate);

  const errorDisplay = errorMessage && (
    <Typography variant="caption" display="block" gutterBottom>
      <Box color={"error.main"}> {errorMessage}</Box>
    </Typography>
  );

  return (
    <form onSubmit={formik.handleSubmit}>
      <Grid container justify="space-evenly">
        <Grid item>
          <DateTimePicker
            id="startDate"
            name="startDate"
            value={formik.values.startDateUtc}
            onChange={(value) => {
              fetchCars();
              fetchEmployees();
              formik.setFieldValue("startDateUtc", value);
            }}
            format="dd/MM/yyyy"
            minDate={new Date()}
            label="Trip start"
            autoOk
            ampm={false}
          />
        </Grid>
        <Grid item>
          <DateTimePicker
            id="endDate"
            name="endDate"
            value={formik.values.endDateUtc}
            onChange={(value) => {
              formik.setFieldValue("endDateUtc", value);
              fetchCars();
              fetchEmployees();
            }}
            format="dd/MM/yyyy"
            minDate={minDate}
            label="Trip End"
            autoOk
            ampm={false}
            minDateMessage={"End date needs to be after start date"}
          />
        </Grid>
      </Grid>
      <Grid container justify="space-evenly">
        <Grid item sm={4} xs={8}>
          <Autocomplete
            id="startLocationId"
            name="startLocationId"
            options={locations}
            getOptionLabel={(option) => option.name}
            renderInput={(params) => (
              <TextField
                {...params}
                variant="standard"
                label="Start Location"
                placeholder="Location"
              />
            )}
            onChange={(e, value) => {
              formik.setFieldValue("startLocationId", value.locationId);
              fetchCars();
            }}
          />
        </Grid>
        <Grid item sm={4} xs={8}>
          <Autocomplete
            id="endLocationId"
            name="endLocationId"
            options={locations}
            getOptionLabel={(option) => option.name}
            renderInput={(params) => (
              <TextField
                {...params}
                variant="standard"
                label="End Location"
                placeholder="Location"
              />
            )}
            onChange={(e, value) => {
              formik.setFieldValue("endLocationId", value.locationId);
            }}
          />
        </Grid>
      </Grid>
      <Container maxWidth={"md"}>
        <Autocomplete
          multiple
          id="employeeIds"
          name="employeeIds"
          options={employeeData}
          getOptionLabel={(option) => option.employeeName}
          renderInput={(params) => (
            <TextField
              {...params}
              variant="standard"
              label="Add employees"
              placeholder="Employee"
            />
          )}
          onChange={(e, value) => {
            const ids = value.map((e) => e.employeeId);
            formik.setFieldValue("employeeIds", ids);
          }}
        />
      </Container>
      <Container>
        <Select
          labelId="carId"
          id="carId"
          name="carId"
          value={formik.values.carId}
          onChange={formik.handleChange}
        >
          <MenuItem value={0} key={0}>
            Select Car
          </MenuItem>
          {cars &&
            cars.map((c) => (
              <MenuItem value={c.carId} key={c.carId}>
                {c.name}
              </MenuItem>
            ))}
        </Select>
      </Container>

      <Container>
        {errorDisplay}
        <Button type={"submit"} variant={"outlined"}>
          Create Travel plan
        </Button>
      </Container>
    </form>
  );
};

export default CreateTravelPlan;
