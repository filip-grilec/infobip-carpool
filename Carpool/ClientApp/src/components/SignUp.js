import { SIGN_IN } from "../routing/routes";

import React, { useState } from "react";
import Avatar from "@material-ui/core/Avatar";
import Button from "@material-ui/core/Button";
import CssBaseline from "@material-ui/core/CssBaseline";
import Link from "@material-ui/core/Link";
import Grid from "@material-ui/core/Grid";
import Box from "@material-ui/core/Box";
import LockOutlinedIcon from "@material-ui/icons/LockOutlined";
import Typography from "@material-ui/core/Typography";
import { makeStyles } from "@material-ui/core/styles";
import Container from "@material-ui/core/Container";
import { useAuth } from "../contexts/AuthContext";
import { useHistory } from "react-router";
import { Link as RouterLink } from "react-router-dom";
import { useFormik } from "formik";
import * as Yup from "yup";
import InputField from "./InputField";

function Copyright() {
  return (
    <Typography variant="body2" color="textSecondary" align="center">
      {"Copyright © "}
      <Link color="inherit" href="https://material-ui.com/">
        Your Website
      </Link>{" "}
      {new Date().getFullYear()}
      {"."}
    </Typography>
  );
}

const useStyles = makeStyles((theme) => ({
  paper: {
    marginTop: theme.spacing(8),
    display: "flex",
    flexDirection: "column",
    alignItems: "center",
  },
  avatar: {
    margin: theme.spacing(1),
    backgroundColor: theme.palette.secondary.main,
  },
  form: {
    width: "100%", // Fix IE 11 issue.
    marginTop: theme.spacing(3),
  },
  submit: {
    margin: theme.spacing(3, 0, 2),
  },
}));

export default function SignUp() {
  const classes = useStyles();

  const { signUp } = useAuth();

  const [errorMessage, setErrorMessage] = useState("");

  const history = useHistory();
  const errorBox = errorMessage && (
    <Grid item xs={12} style={{ textAlign: "center" }}>
      <Typography variant={"body2"}>
        <Box color={"error.main"}>{errorMessage}</Box>
      </Typography>
    </Grid>
  );

  const formik = useFormik({
    initialValues: {
      firstName: "",
      lastName: "",
      email: "",
      password: "",
      passwordConfirmation: "",
    },
    validationSchema: Yup.object({
      email: Yup.string().email("Invalid email address").required("Required"),
      password: Yup.string()
        .min(6, "Minimum 6 characters")
        .required("Required"),
      lastName: Yup.string().required("Required"),
      firstName: Yup.string().required("Required"),
      passwordConfirmation: Yup.string().oneOf(
        [Yup.ref("password"), null],
        "Passwords must match"
      ),
    }),
    onSubmit: async ({ email, password }) => {
      try {
        setErrorMessage("");
        await signUp(email, password);
        history.push("/");
      } catch ({ message }) {
        setErrorMessage(message);
      }
    },
  });

  return (
    <Container component="main" maxWidth="xs">
      <CssBaseline />
      <div className={classes.paper}>
        <Avatar className={classes.avatar}>
          <LockOutlinedIcon />
        </Avatar>
        <Typography component="h1" variant="h5">
          Sign up
        </Typography>
        <form className={classes.form} onSubmit={formik.handleSubmit}>
          <Grid container spacing={2}>
            <Grid item xs={12} sm={6}>
              <InputField
                formik={formik}
                field={"firstName"}
                text={"First Name"}
                autoComplete={"given-name"}
              />
            </Grid>
            <Grid item xs={12} sm={6}>
              <InputField
                formik={formik}
                text={"Last Name"}
                field={"lastName"}
                autoComplete={"family-name"}
              />
            </Grid>
            <Grid item xs={12}>
              <InputField
                field={"email"}
                text={"Email"}
                autoComplete={"email"}
                formik={formik}
              />
            </Grid>
            <Grid item xs={12}>
              <InputField
                field={"password"}
                text={"Password"}
                autoComplete={"current-password"}
                formik={formik}
                type={"password"}
              />
            </Grid>
            <Grid item xs={12}>
              <InputField
                field={"passwordConfirmation"}
                text={"Confirm Password"}
                type={"password"}
                formik={formik}
              />
            </Grid>
            {errorBox}
          </Grid>
          <Button
            type="submit"
            fullWidth
            variant="contained"
            color="primary"
            className={classes.submit}
          >
            Sign Up
          </Button>
          <Grid container justify="flex-end">
            <Grid item>
              <RouterLink to={SIGN_IN}>
                Already have an account? Sign in
              </RouterLink>
            </Grid>
          </Grid>
        </form>
      </div>
      <Box mt={5}>
        <Copyright />
      </Box>
    </Container>
  );
}
