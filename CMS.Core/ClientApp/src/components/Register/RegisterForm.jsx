import React, { useState, useEffect, useContext } from 'react';
import {useHistory} from 'react-router-dom';
import { Link, useHistory } from 'react-router-dom';
import axios from '../../axios';
import { Form, Input, Button, Checkbox, Alert, Spin } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';

const initialFormState = {
    firstName: "",
    lastName:"",
    email:"",
    username: "",   
    password:"",
    passwordConfirm: ""
}

const RegisterForm = props => {

    const history = useHistory();

    const [isLoading, setIsLoading] = useState(false);
    const [formState, setFormState] =  useState(initialFormState);

    const handleChangeFormState = (value, prop) =>{
        let data = {...formState};
        data[prop] = value;
        setFormState(data);
    }

    const handleSubmit = () => {
        const data = {...formState};
        data.password = btoa(data.password);
        delete data.passwordConfirm;

        try{
            const response = await axios.post('/api/v1/identity/register', data);
            
            if(response.status == 200){
                history.push("/login");
            }
        }
        catch(e){
            const response = e.response;
            
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
            >
                <h2>Register</h2>

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
                    rules={[{ required: true, message: 'Please input your E-Mail!' }]}
                >
                    <Input
                        onChange={e => handleChangeFormState(e.target.value, "email")}
                        
                    />
                </Form.Item>

                <Form.Item
                    name="password"
                    label="Password"
                    value={formState.password}
                    rules={[{ required: true, message: 'Please input your Last Name!' }]}
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
                    rules={[{ required: true, message: 'Please input your Last Name!' }]}
                >
                    <Input
                        onChange={e => handleChangeFormState(e.target.value, "confirmPassword")}
                        
                    />
                </Form.Item>
                
                {/* <Form.Item>
                    Already have an account?
                    <Link to="/login" className="login-form-register-link">Sign In</Link>
                </Form.Item> */}



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


