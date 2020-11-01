import React, {useEffect, useState} from 'react';
import { useParams, Link } from 'react-router-dom';
import { Card, Spin, Button, Col, Row } from 'antd';

const MessageDisplayCard = (props) => {

    const textWrap = {
        whiteSpace:"pre-wrap",
        wordWrap: "break-word"
    };

    const titleColor={
        backgroundColor: props.isOwnMessage !== false ? '#91d5ff' : 'white'
    };


    
    console.log(props.record);
    
    const sender = `${props.record.senderName} (${props.record.senderEmail})`
    return(
        <Card title={sender} headStyle={titleColor}>
            <div style={textWrap} >
                {props.record.messageContent}
            </div>
            
        </Card>
    )
}

export default MessageDisplayCard;