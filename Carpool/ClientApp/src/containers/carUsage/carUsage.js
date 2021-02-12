import React, { useEffect, useState } from "react";
import { Container } from "@material-ui/core";
import { useAuth } from "../../contexts/AuthContext";
import { getStatistics } from "../../axios/carpoolAxios";
import CarStatistic from "../../components/carStatistic";

const CarUsage = () => {
  const { userLoaded } = useAuth();
  const [carStatistics, setCarStatistics] = useState([]);
  useEffect(() => {
    if (userLoaded) {
      getStatistics().then(({ data }) => {
        console.log(data);
        setCarStatistics(data);
      });
    }
  }, [userLoaded]);
  return (
    <Container>
      {(carStatistics &&
        carStatistics.map((statistic) => (
          <CarStatistic
            key={statistic.car.carId}
            car={statistic.car}
            trips={statistic.travelPlanStatistics}
          />
        ))) || <p>Loading</p>}
    </Container>
  );
};

export default CarUsage;
