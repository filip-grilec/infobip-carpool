import React from "react";
import CarStatistic from "./carStatistic";

const employees = [
  {
    employeeId: 1,
    employeeName: "Danijela Šabarić",
    hasDriverLicence: false,
  },
  {
    employeeId: 2,
    employeeName: "Davor TkalčićDulić",
    hasDriverLicence: true,
  },
  {
    employeeId: 4,
    employeeName: "Zdeslava Rauš",
    hasDriverLicence: true,
  },
  {
    employeeId: 5,
    employeeName: "Lena Valcer",
    hasDriverLicence: true,
  },
  {
    employeeId: 6,
    employeeName: "Rajna VesanovićDvornik",
    hasDriverLicence: false,
  },
  {
    employeeId: 7,
    employeeName: "Staša Celjak",
    hasDriverLicence: true,
  },
  {
    employeeId: 9,
    employeeName: "Gordana Poplaša",
    hasDriverLicence: false,
  },
  {
    employeeId: 10,
    employeeName: "Dorana Maroš",
    hasDriverLicence: true,
  },
  {
    employeeId: 11,
    employeeName: "Bruno Macko",
    hasDriverLicence: false,
  },
  {
    employeeId: 12,
    employeeName: "Mica Magdika",
    hasDriverLicence: true,
  },
  {
    employeeId: 13,
    employeeName: "Ema Kuba",
    hasDriverLicence: true,
  },
  {
    employeeId: 14,
    employeeName: "Slavica Rajačić",
    hasDriverLicence: true,
  },
  {
    employeeId: 15,
    employeeName: "Dada Lovač",
    hasDriverLicence: true,
  },
  {
    employeeId: 16,
    employeeName: "Matko Škorjanec",
    hasDriverLicence: false,
  },
  {
    employeeId: 17,
    employeeName: "Dona Šijak",
    hasDriverLicence: true,
  },
  {
    employeeId: 18,
    employeeName: "Bela Korpar",
    hasDriverLicence: false,
  },
  {
    employeeId: 19,
    employeeName: "Filip Namjestnik",
    hasDriverLicence: false,
  },
  {
    employeeId: 20,
    employeeName: "Slavek ĐuračkiĆosić",
    hasDriverLicence: true,
  },
  {
    employeeId: 21,
    employeeName: "Ruža Španić",
    hasDriverLicence: true,
  },
  {
    employeeId: 22,
    employeeName: "Branimir Vangelovski",
    hasDriverLicence: true,
  },
  {
    employeeId: 23,
    employeeName: "Tončica Vukadin",
    hasDriverLicence: true,
  },
  {
    employeeId: 24,
    employeeName: "Ana Trvalovski",
    hasDriverLicence: true,
  },
  {
    employeeId: 25,
    employeeName: "Grga Sprečaković",
    hasDriverLicence: true,
  },
];
const cars = [
  {
    carId: 1,
    name: "XC90",
    carType: "Wagon",
    color: "red",
    licencePlate: "ZG 5833-GD",
    seats: 5,
  },
  {
    carId: 2,
    name: "Jetta",
    carType: "Convertible",
    color: "plum",
    licencePlate: "ZG 7307-JV",
    seats: 5,
  },
  {
    carId: 3,
    name: "Alpine",
    carType: "Coupe",
    color: "grey",
    licencePlate: "ZG 5423-HM",
    seats: 5,
  },
  {
    carId: 4,
    name: "Beetle",
    carType: "Wagon",
    color: "olive",
    licencePlate: "DA 5832-JD",
    seats: 5,
  },
  {
    carId: 5,
    name: "Grand Cherokee",
    carType: "Wagon",
    color: "grey",
    licencePlate: "DA 9590-UF",
    seats: 5,
  },
  {
    carId: 6,
    name: "Malibu",
    carType: "Cargo Van",
    color: "salmon",
    licencePlate: "KA 2833-EQ",
    seats: 4,
  },
  {
    carId: 7,
    name: "Grand Caravan",
    carType: "Convertible",
    color: "ivory",
    licencePlate: "ST 7364-PN",
    seats: 5,
  },
  {
    carId: 8,
    name: "Durango",
    carType: "Cargo Van",
    color: "gold",
    licencePlate: "DA 6340-PB",
    seats: 4,
  },
  {
    carId: 9,
    name: "Model T",
    carType: "Minivan",
    color: "maroon",
    licencePlate: "OS 2157-MR",
    seats: 5,
  },
  {
    carId: 10,
    name: "Durango",
    carType: "Hatchback",
    color: "magenta",
    licencePlate: "ST 3938-EO",
    seats: 5,
    trips: [{}],
  },
];

const CarStatistics = (props) => {
  return cars.map((car) => (
    <CarStatistic key={car.carId} car={car} employees={employees} />
  ));
};

export default CarStatistics;
