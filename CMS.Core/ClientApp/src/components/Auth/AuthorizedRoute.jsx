import React, { Component, useEffect, useState } from 'react';
import { Route, Link, Redirect} from 'react-router-dom';

import axios from '../../axios';

const AuthorizedRoute = ({ component: Component, ...rest }) => {

    const [authState, setAuthState] = useState(true);
    const [isCallFinished, setIsCallFinished] = useState(false);

    const checkAuthStatus = async () => {
        const token = localStorage.getItem('token');
        console.log(token);
        if(token === null){
            setAuthState(false);
            setIsCallFinished(true);
        }

        try{
            const request = await axios.get('/api/v1/identity/validate');

            setAuthState(request.data);
            setIsCallFinished(true);
        }
        catch(e){
            const response = e.response;

            setAuthState(false);
            setIsCallFinished(true);
        }

        
    }

    useEffect(() => {
        checkAuthStatus();
    },[])

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