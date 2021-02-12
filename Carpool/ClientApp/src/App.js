import "./App.css";
import Dashboard from "./layout/Dashboard";
import React from "react";
import { BrowserRouter } from "react-router-dom";
import { MuiPickersUtilsProvider } from "@material-ui/pickers";
import DateFnsUtils from "@date-io/date-fns";

function App() {
  return (
    <BrowserRouter>
      <MuiPickersUtilsProvider utils={DateFnsUtils}>
        <Dashboard />
      </MuiPickersUtilsProvider>
    </BrowserRouter>
  );
}

export default App;
