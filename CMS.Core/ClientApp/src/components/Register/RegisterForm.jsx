import React, { useState, useEffect, useContext } from 'react';
import { Link, useHistory } from 'react-router-dom';
import axios from '../../axios';
import { Form, Input, Button, Checkbox, Alert, Spin } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';

import RegistrationResponse from './RegistrationResponse';

const initialFormState = {
    firstName: "",
    lastName:"",
    email:"",
    username: "",   
    password:"",
    passwordConfirm: ""
}

const initialMessageState = {
    show: false,
    message: "",
    type:"error"
};

const RegisterForm = props => {

    const history = useHistory();

    const [isLoading, setIsLoading] = useState(false);
    const [formState, setFormState] =  useState(initialFormState);

    const [alertMessage, setAlertMessage] = useState(initialMessageState);

    const handleChangeFormState = (value, prop) =>{
        let data = {...formState};
        data[prop] = value;
        setFormState(data);
    }

    const handleSubmit = async () => {
        setIsLoading(true);
        const data = {...formState};
        data.password = btoa(data.password);
        delete data.passwordConfirm;

        console.log(data);

        try{
            const response = await axios.post('/api/v1/identity/register', data);
            
            if(response.status == 200){

                let alertLocal={...alertMessage};
                alertLocal.message="Registration successful!";
                alertLocal.type="success";
                alertLocal.show=true;
                setAlertMessage(alertLocal);

                setTimeout(history.push("/login"), 1000);
                
            }

            setIsLoading(false);
        }
        catch(e){
            const response = e.response;
            
            let alertLocal = {...alertMessage};
            alertLocal.show= true;
            alertLocal.type="error";

            if(response.status === 400){
                switch(response.data){
                    case RegistrationResponse.USERNAME_EXISTS:
                        alertLocal.message = "Username already exists!";
                        break;
                    case RegistrationResponse.EMAIL_EXISTS:
                        alertLocal.message = "Email already exists!";
                        break;
                    case RegistrationResponse.EMAIL_USERNAME_EXIST:
                        alertLocal.message = "Email and Username already exist!";
                        break;
                    default: 
                        break;
                }
            }
            else{
                alertLocal.message="An error occured."
            }
            setAlertMessage(alertLocal);
            setIsLoading(false);
        }
    }

    const formLayout = {
        labelCol: { span: 4 },
        wrapperCol: { span: 14 },
    }

    return(
        <Spin spinning={isLoading}>
            <Form
                name="register"
                layout="vertical"
                onFinish={handleSubmit}
            >
                <h2>Register</h2>

                { alertMessage.show === true ? <Alert message={alertMessage.message} type={alertMessage.type} /> : null}

                <Form.Item
                    name="firstName"
                    label="First Name"
                    value={formState.firstName}
                    rules={[{ required: true, message: 'Please input your First Name!' }]}
                >
                    <Input
                        onChange={e => handleChangeFormState(e.target.value, "firstName")}
                    />
                </Form.Item>
                <Form.Item
                    name="lastName"
                    label="Last Name"
                    value={formState.lastName}
                    rules={[{ required: true, message: 'Please input your Last Name!' }]}
                >
                    <Input
                        onChange={e => handleChangeFormState(e.target.value, "lastName")}
                    />
                </Form.Item>

                <Form.Item
                    name="username"
                    label="Username"
                    value={formState.username}
                    rules={[{ required: true, message: 'Please input your Username!' }]}
                >
                    <Input
                        onChange={e => handleChangeFormState(e.target.value, "username")}
                    />
                </Form.Item>

                <Form.Item
                    name="email"
                    label="E-Mail"
                    value={formState.email}
                    rules={[
                            { required: true, message: 'Please input your E-Mail!' },
                            {type:"email",message:"Invalid E-Mail"}
                        ]}
                >
                    <Input
                        onChange={e => handleChangeFormState(e.target.value, "email")}
                        
                    />
                </Form.Item>

                <Form.Item
                    name="password"
                    label="Password"
                    value={formState.password}
                    rules={[{ required: true, message: 'Please input your Password!' }]}
                >
                    <Input
                        type="password"
                        onChange={e => handleChangeFormState(e.target.value, "password")}
                        
                    />
                </Form.Item>

                <Form.Item
                    name="passwordRepeat"
                    label="Confirm Password"
                    value={formState.passwordConfirm}
                    rules={[
                        { required: true, message: 'Please repeat your password.' },
                        ({ getFieldValue }) => ({
                            validator(rule, value) {
                            if (!value || getFieldValue('password') === value) {
                                return Promise.resolve();
                            }
                            return Promise.reject('Passwords must match!');
                            },
                        })
                    ]}
                >
                    <Input
                        type="password"
                        onChange={e => handleChangeFormState(e.target.value, "passwordConfirm")}
                        
                    />
                </Form.Item>
                
                <Form.Item>
                    <Link to="/login" className="login-form-register-link">&nbsp;Sign In</Link>

                    <p className="login-form-register-link">
                        Already have an account? 
                    </p>
                    
                    
                </Form.Item>



                <Form.Item>
                    <Button type="primary" htmlType="submit" className="login-form-button" >
                        Register
                    </Button>
                </Form.Item>

            </Form>
        </Spin>
    )
}


export default RegisterForm;


