import React from "react";
import List from "@material-ui/core/List";
import ListItem from "@material-ui/core/ListItem";
import ListItemIcon from "@material-ui/core/ListItemIcon";
import ListItemText from "@material-ui/core/ListItemText";
import {
  getCollapseBtn,
  getDrawerSidebar,
  getSidebarContent,
} from "@mui-treasury/layout";
import styled from "styled-components";
import {
  ExitToApp as SignOutIcon,
  DirectionsCar as CarIcon,
  BarChart,
} from "@material-ui/icons";
import { useAuth } from "../contexts/AuthContext";
import { useHistory } from "react-router";
import { CAR_USAGE, CARPOOL, GRATEFULNESS, SIGN_IN } from "../routing/routes";
import { NavLink } from "react-router-dom";

const DrawerSidebar = getDrawerSidebar(styled);
const SidebarContent = getSidebarContent(styled);
const CollapseBtn = getCollapseBtn(styled);

const styles = {
  removeUnderline: {
    color: "inherit",
    textDecoration: "inherit",
  },
};

const SidebarDrawer = (props) => {
  const { signOut, currentUser } = useAuth();
  const history = useHistory();

  function signOutHandler() {
    signOut();
    history.push(SIGN_IN);
  }

  const accountText = currentUser ? "Sign out" : "Log in";

  const AccountAction = ({ accountText }) => (
    <ListItem button onClick={signOutHandler}>
      <ListItemIcon>
        <SignOutIcon />
      </ListItemIcon>
      <ListItemText primary={accountText} />
    </ListItem>
  );

  const NavigationLink = ({ text, Icon, route, selected }) => (
    <NavLink to={route} style={styles.removeUnderline}>
      <ListItem button selected={selected}>
        <ListItemIcon>
          <Icon />
        </ListItemIcon>
        <ListItemText primary={text} />
      </ListItem>
    </NavLink>
  );

  return (
    <DrawerSidebar sidebarId="unique_id">
      <SidebarContent>
        <List component="nav">
          <NavigationLink text={"Create Trip"} Icon={CarIcon} route={CARPOOL} />
          <NavigationLink
            text={"Car Usage"}
            Icon={BarChart}
            route={CAR_USAGE}
          />

          <AccountAction accountText={accountText} />
        </List>
      </SidebarContent>
      <CollapseBtn />
    </DrawerSidebar>
  );
};

export default SidebarDrawer;
