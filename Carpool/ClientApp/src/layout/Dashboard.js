import React from "react";
import styled from "styled-components";
import CssBaseline from "@material-ui/core/CssBaseline";
import { FooterMockUp } from "@mui-treasury/mockup/layout";
import Layout, {
  getContent,
  getFooter,
  getHeader,
  getSidebarTrigger,
  Root,
} from "@mui-treasury/layout";
import SidebarDrawer from "./SidebarDrawer";
import { Redirect, Route, Switch } from "react-router-dom";

import { CAR_USAGE, CARPOOL, SIGN_IN, SIGN_UP } from "../routing/routes";
import SignIn from "../components/SignIn";
import AuthProvider from "../contexts/AuthContext";
import PrivateRoute from "../routing/PrivateRoute";
import SignUp from "../components/SignUp";
import Gratefulness from "../containers/greatefulness/gratefullness";
import Carpool from "../containers/carpool/carpool";
import CarUsage from "../containers/carUsage/carUsage";

const Content = getContent(styled);
const Footer = getFooter(styled);
const Header = getHeader(styled);
const SidebarTrigger = getSidebarTrigger(styled);

const scheme = Layout();

scheme.configureHeader((builder) => {
  builder
    .registerConfig("xs", {
      position: "sticky",
    })
    .registerConfig("md", {
      position: "relative", // won't stick to top when scroll down
    });
});

scheme.configureEdgeSidebar((builder) => {
  builder
    .create("unique_id", { anchor: "left" })
    .registerTemporaryConfig("xs", {
      anchor: "left",
      width: "auto", // 'auto' is only valid for temporary variant
    })
    .registerPermanentConfig("md", {
      width: 256, // px, (%, rem, em is compatible)
      collapsible: true,
      collapsedWidth: 64,
    });
});

scheme.enableAutoCollapse("unique_id", "md");

const Dashboard = () => (
  <AuthProvider>
    <Root scheme={scheme}>
      {({ state: { sidebar } }) => (
        <>
          <CssBaseline />
          <Header>
            <SidebarTrigger sidebarId="unique_id" />
          </Header>
          <SidebarDrawer />
          <Content>
            <Switch>
              <Route path={SIGN_IN} component={SignIn} />
              <Route path={SIGN_UP} component={SignUp} />
              <PrivateRoute path={CARPOOL} component={Carpool} />
              <PrivateRoute path={CAR_USAGE} component={CarUsage} />
            </Switch>
          </Content>
          <Footer>
            <FooterMockUp />
          </Footer>
        </>
      )}
    </Root>
  </AuthProvider>
);

export default Dashboard;
