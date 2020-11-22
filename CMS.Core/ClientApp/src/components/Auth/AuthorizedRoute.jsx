import React, { Component, useEffect, useState } from 'react';
import { Route, Link, Redirect} from 'react-router-dom';

import axios from '../../axios';

const AuthorizedRoute = ({ component: Component, ...rest }) => {

    const [authState, setAuthState] = useState(false);
    const [isCallFinished, setIsCallFinished] = useState(false);

    const checkAuthStatus = async () => {
        const token = localStorage.getItem('token');
        if(token === null){
            setAuthState(false);
            setIsCallFinished(true);
        } 

        const request = await axios.get('/api/v1/identity/validate');

        setAuthState(request.data);
        setIsCallFinished(true);
    }

    useEffect(() => {
        checkAuthStatus();
    },[isCallFinished])

    console.log(authState);

    const RenderRoute = () =>
        <Route {...rest} render={(props) => (
            authState === true
                ? <Component {...props} />
                : <Redirect to='/login' />
        )} />;

    return(
        isCallFinished === true ? <RenderRoute /> : null
    )
}

export default AuthorizedRoute;