import React  from 'react';
import { Card } from 'antd';

const MessageDisplayCard = (props) => {

    const textWrap = {
        whiteSpace:"pre-wrap",
        wordWrap: "break-word"
    };

    const titleColor={
        backgroundColor: props.record.isOwnMessage === false ? 'white' : '#91d5ff'
    };

    const cardStyle={
        marginBottom: '20px'
    }

    
    
    const sender = `${props.record.senderName} (${props.record.senderEmail})`

    const CardTitle = () => {
        const dateFormatted = props.record.timestamp.replace("T"," ")
        return(
            <div>
                <b>{sender}</b>
                <p style={{marginBottom:"0px"}}>At: {dateFormatted}</p>
            </div>

            
        )
    }

    return(
        <Card title={<CardTitle />} headStyle={titleColor} style={cardStyle}>
            <div style={textWrap} >
                {props.record.messageContent}
            </div>
            
        </Card>
    )
}

export default MessageDisplayCard;