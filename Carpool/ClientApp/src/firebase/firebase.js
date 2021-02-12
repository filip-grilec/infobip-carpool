import firebase from "firebase";
const firebaseConfig = {
  apiKey: "AIzaSyBuyhpslAsI4XYed1DGjNuQJISjbVYPbRQ",
  authDomain: "carpool-722b3.firebaseapp.com",
  projectId: "carpool-722b3",
  storageBucket: "carpool-722b3.appspot.com",
  messagingSenderId: "546872155329",
  appId: "1:546872155329:web:4bbed602137441ec428840",
};
const app = firebase.initializeApp(firebaseConfig);

export const auth = app.auth();
export default app;
