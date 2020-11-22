import React, {useState, useEffect} from 'react';
import {Card} from 'antd';

import LoginForm from './LoginForm';
import AlreadyLoggedIn from './AlreadyLoggedIn';

const Login = props => {    

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

    const LoginFormDisplay = () => 
        authState !== null
        ? <LoginForm />
        : <AlreadyLoggedIn />
    

    return (
        <Card className="login-card">
            {isCallFinished === true ? LoginFormDisplay : null}
        </Card>
        
    )
};

export default Login;