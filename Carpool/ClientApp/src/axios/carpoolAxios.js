import axios from "axios";
// Set config defaults when creating the okrappAxios
const carpoolAxios = axios.create({
  baseURL: "/api",
});

export const getGratefulnessEntries = () => {
  return carpoolAxios.get("gratefulness");
};

export const getCars = () => {
  return carpoolAxios.get("cars");
};

export const getEmployees = (startDate, endDate) => {
  const start = new Date(startDate).toJSON();
  const end = new Date(endDate).toJSON();
  return carpoolAxios.get(`employees?startDateUtc=${start}&endDateUtc=${end}`);
};
export const createTravelPlan = (travelPlanOptions) =>
  carpoolAxios.post("travelplan", travelPlanOptions);

export const getAvailableCars = (travelPlanOptions) =>
  carpoolAxios.post("cars", travelPlanOptions);

export const getLocations = () => carpoolAxios.get("locations");

export const getStatistics = () => carpoolAxios.get("statistics");
export default carpoolAxios;
