import React, { useContext, useEffect, useState } from "react";
import { auth } from "../firebase/firebase";
import carpoolAxios from "../axios/carpoolAxios";
import { useAuthState } from "react-firebase-hooks/auth";

const AuthContext = React.createContext();

export function useAuth() {
  return useContext(AuthContext);
}

export function AuthProvider({ children }) {
  const [user, loading, error] = useAuthState(auth);
  const [tokenSet, setTokenSet] = useState(false);

  function signUp(email, password) {
    return auth.createUserWithEmailAndPassword(email, password);
  }

  function signIn(email, password) {
    return auth.signInWithEmailAndPassword(email, password);
  }

  function signOut() {
    return auth.signOut();
  }
  function setToken() {
    auth.currentUser.getIdToken().then((token) => {
      carpoolAxios.defaults.headers.common["Authorization"] = `Bearer ${token}`;
      setTokenSet(true);
    });
  }

  function deleteToken() {
    carpoolAxios.defaults.headers.common["Authorization"] = null;
    setTokenSet(false);
  }

  const userLoaded = !!tokenSet;

  useEffect(() => {
    return auth.onAuthStateChanged((user) => {
      if (user) {
        setToken();
      } else {
        deleteToken();
      }
    });
  }, [user, tokenSet]);

  const value = {
    currentUser: user,
    signUp,
    signIn,
    signOut: signOut,
    loadingUser: loading,
    userLoaded: userLoaded,
  };
  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export default AuthProvider;
