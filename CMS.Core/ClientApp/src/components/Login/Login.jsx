import React, {useState, useEffect} from 'react';
import {Card} from 'antd';

import LoginForm from './LoginForm';
import AlreadyLoggedIn from './AlreadyLoggedIn';

const Login = props => {    

    const token = localStorage.getItem('token');

    return (
        <Card className="login-card">
            {token === null ? <LoginForm /> : <AlreadyLoggedIn />}
        </Card>
        
    )
};

export default Login;