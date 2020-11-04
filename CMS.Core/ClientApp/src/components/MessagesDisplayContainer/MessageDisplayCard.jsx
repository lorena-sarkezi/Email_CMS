import React  from 'react';
import { Card } from 'antd';

const MessageDisplayCard = (props) => {

    const textWrap = {
        whiteSpace:"pre-wrap",
        wordWrap: "break-word"
    };

    const titleColor={
        backgroundColor: props.record.isOwnMessage === true ? 'white' : '#91d5ff'
    };

    const cardStyle={
        marginBottom: '20px'
    }

    console.log(titleColor);
    
    console.log(props.record);
    
    const sender = `${props.record.senderName} (${props.record.senderEmail})`
    return(
        <Card title={sender} headStyle={titleColor} style={cardStyle}>
            <div style={textWrap} >
                {props.record.messageContent}
            </div>
            
        </Card>
    )
}

export default MessageDisplayCard;