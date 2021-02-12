import React from "react";
import TextField from "@material-ui/core/TextField";

const InputField = ({ field, text, autoComplete, formik, type = "text" }) => (
    <TextField
      variant="outlined"
      required
      fullWidth
      name={field}
      label={text}
      id={field}
      autoComplete={autoComplete}
      onChange={formik.handleChange}
      value={formik.values[field]}
      error={formik.touched[field] && !!formik.errors[field]}
      helperText={formik.errors[field]}
      onBlur={formik.handleBlur}
      type={type}
    />
);

export default InputField;
