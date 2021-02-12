import React, { useEffect } from "react";
import {getCars, getGratefulnessEntries} from "../../axios/carpoolAxios";
import { useAuth } from "../../contexts/AuthContext";

const Gratefulness = (props) => {
  const { userLoaded } = useAuth();
  useEffect(() => {
    if (userLoaded) {
      getCars().then(({ data }) => {
        console.log(data);
      });
    }
  }, [userLoaded]);

  return <div></div>;
};

export default Gratefulness;
