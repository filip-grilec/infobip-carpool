import React from "react";
import { useAuth } from "../contexts/AuthContext";
import { Redirect, Route } from "react-router";
import { SIGN_IN } from "./routes";
import Skeleton from "react-loading-skeleton";

const PrivateRoute = ({ component: Component, ...rest }) => {
  const { currentUser, loadingUser } = useAuth();

  const loadingMessage = <Skeleton count={15} />;

  const userNotLoggedIn = !currentUser;
  const finishedLoading = !loadingUser;

  return (
    <Route
      {...rest}
      render={(props) => {
        if (finishedLoading && userNotLoggedIn) {
          return <Redirect to={SIGN_IN} />;
        }

        return loadingUser ? loadingMessage : <Component {...props} />;
      }}
    />
  );
};

export default PrivateRoute;
