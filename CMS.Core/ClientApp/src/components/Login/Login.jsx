import React, {useState, useEffect} from 'react';
import {Card} from 'antd';

import LoginForm from './LoginForm';

const Login = props => {    

    return (
        <Card className="login-card">
            <LoginForm />
        </Card>
        
    )
};

export default Login;