import React, {useState, useEffect} from 'react';
import {Card, Spin} from 'antd';
import axios from '../../axios';


import AlreadyRegistered from './AlreadyRegistered';
import RegisterForm from './RegisterForm';

const Register = props => {
    const [authState, setAuthState] = useState(false);
    const [isCallFinished, setIsCallFinished] = useState(false);

    const checkAuthStatus = async () => {
        const token = localStorage.getItem('token');
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
    },[isCallFinished, authState])

    const RegisterSpinner = () => {
        return(
            <div className="div-text-content-centered">
                <Spin />
            </div>
        )
    }

    const RegisterFormDisplay = () => {
        if(isCallFinished === true){
            return(
                authState === false
                ? <RegisterForm />
                : <AlreadyRegistered />
            )
            
        }
        return <RegisterSpinner />;
    }

    return (
        <Card className="login-card">
            <RegisterFormDisplay />
        </Card>
        
    )
}

export default Register;