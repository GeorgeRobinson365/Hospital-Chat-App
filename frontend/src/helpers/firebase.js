import firebase from 'firebase/compat/app';
import {getAuth, 
  onAuthStateChanged, 
  GoogleAuthProvider, 
  OAuthProvider, 
  signInWithPopup,
} from 'firebase/auth';

const firebaseConfig = {
    apiKey: "AIzaSyBNH_J_Ennc0zAcjQnl-EYQ2DWc8WCpiTo",
    authDomain: "healthcare-web-portal.firebaseapp.com",
    projectId: "healthcare-web-portal",
    storageBucket: "healthcare-web-portal.appspot.com",
    messagingSenderId: "1050469182717",
    appId: "1:1050469182717:web:b9ac85d41fb9de66a7a294"
  }

try {
    firebase.initializeApp(firebaseConfig);
} catch (err) {
    if (!/already exists/.test(err.message)) {
        console.error('Firebase initialization error', err.stack);
    }
}

export const auth = getAuth(firebase.app());

export const isUserExpired = () => {
  const fbUser = auth.currentUser
  console.log(fbUser)
  if (fbUser) {
    const now = new Date()
    if (fbUser.stsTokenManager && fbUser.stsTokenManager.expirationTime) {
      const expirationTime = new Date(fbUser.stsTokenManager.expirationTime)
      if (expirationTime < now) {
        log.info('Refreshing User Token')
        fbUser
          .getIdToken(true)
          .then((idToken) => {
            log.info('Token refreshed')
            user.updateToken(idToken)
            fbUser.reload()
            return Promise.resolve(idToken)
          })
          .catch((error) => {
            log.error(`Failed to update user, redirecting to login: ${error}`)
            showErrorNotification('Failed to update user, redirecting to login')
            logOut()
            return Promise.reject(error)
          })
      }
    }
  }
}
