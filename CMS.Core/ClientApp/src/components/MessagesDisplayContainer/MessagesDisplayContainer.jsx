import React, {useEffect, useState} from 'react';
import { useParams, Link, useHistory } from 'react-router-dom';
import { Card, Spin, Button, Col, Row } from 'antd';

import LeftOutlined from '@ant-design/icons/LeftOutlined';
import MailTwoTone from '@ant-design/icons/MailTwoTone';

import MessageDisplayCard from './MessageDisplayCard';
import MessageReplyModal from './MessageReplyModal';

import axios from '../../axios';



const LoadingSpinner = () => {
    return(
        <div style={{textAlign:"center"}}>
            <Spin size="large" style={{color:"white"}}/>
        </div>
    )
}


const MessagesDisplayContainer = (props) =>{
    if(localStorage.getItem("modal") ===null)
        localStorage.setItem("modal",false);


    const modalBool = localStorage.getItem("modal") == 'true' ? true : false;
    console.log(modalBool);

    const {threadId} = useParams();

    const history = useHistory();

    const [dummyState, setDummyState] = useState(false);
    const [threadData, setThreadData] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [showReplyModal, setShowReplyModal] = useState(modalBool);

    const getThreadMessages = async () =>{
        try{
            setIsLoading(true);
            const response = await axios.get(`/api/v1/messaging/threads/${threadId}`);
            setIsLoading(false);

            setThreadData(response.data);
        }
        catch(e){
            const response = e.response;
            if(response.status === 404){
                history.push("/notfound");
            }
            setIsLoading(false);
        }
    }

    const visibleHandler= (state) => {
        console.log("Visble handler ");
        console.log(state);
        localStorage.setItem("modal",state);

        setShowReplyModal(state);
        if(state === false) 
            setDummyState(!dummyState);
    }

    useEffect(() =>{
        console.log("Render");
        getThreadMessages();
    }, [])

    return(
        <React.Fragment>
            <MessageReplyModal 
                visible={showReplyModal} 
                visibleHandler={visibleHandler} 
                threadId={threadId} 
                dummmyState={dummyState}
                dummyStateCallback={setDummyState}/>

            <Row style={{marginBottom:"5px"}}>
                <Col flex="0 1 auto">
                    <Link to="/">
                        <Button tyle="default" icon={<LeftOutlined />}>
                            Back
                        </Button> 
                    </Link>
                </Col>

                <Col flex="auto"/>

                <Col flex="0 1 auto">
                    <Link to="#" >
                        <Button tyle="default" icon={<MailTwoTone />} onClick={() => visibleHandler(true)}>
                            Reply
                        </Button> 
                    </Link>
                </Col>

            </Row>
            {
                isLoading === true ? <LoadingSpinner /> : null 
            }
            {
                threadData !== null ? threadData.messages.map((value, index) => <MessageDisplayCard record={value}/>) : null
            }
            
        </React.Fragment>
        
    )
}

export default MessagesDisplayContainer;