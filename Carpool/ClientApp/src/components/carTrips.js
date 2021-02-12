import React from "react";
import {
  Avatar,
  makeStyles,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  Tooltip,
} from "@material-ui/core";

const useStyles = makeStyles((theme) => ({
  table: {
    minWidth: 650,
  },
  smallIcon: {
    width: theme.spacing(3),
    height: theme.spacing(3),
  },
}));

const options = {
  weekday: "long",
  year: "numeric",
  month: "long",
  day: "numeric",
};
const displayDate = (dateString) => {
  const date = new Date(dateString);

  return date.toLocaleDateString("hr-HR", options);
};

const CarTrips = ({ trips }) => {
  const classes = useStyles();
  console.log(trips);
  return (
    <div>
      <Table className={classes.table} aria-label="simple table">
        <TableHead>
          <TableRow>
            <TableCell>Start Date</TableCell>
            <TableCell align="right">End Date</TableCell>
            <TableCell align="right">Start Location</TableCell>
            <TableCell align="right">EndLcoation</TableCell>
            <TableCell align="right">Employees</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {trips &&
            trips.map((trip) => (
              <TableRow>
                <TableCell component="th" scope="row">
                  {displayDate(trip.startTimeUtc)}
                </TableCell>
                <TableCell align="right">
                  {displayDate(trip.endTimeUtc)}
                </TableCell>
                <TableCell align="right">{trip.startLocation}</TableCell>
                <TableCell align="right">{trip.endLocation}</TableCell>
                <TableCell align="right">
                  {trip.employees &&
                    trip.employees.map((e) => (
                      <Tooltip title={e.employeeName}>
                        <Avatar className={classes.smallIcon}>
                          {e.avatar}
                        </Avatar>
                      </Tooltip>
                    ))}
                </TableCell>
              </TableRow>
            ))}
        </TableBody>
      </Table>
    </div>
  );
};

export default CarTrips;
