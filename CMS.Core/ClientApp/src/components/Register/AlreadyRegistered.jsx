import React from 'react';
import {Link} from 'react-router-dom';

const AlreadyRegistered = props => 
    <div className='div-text-content-centered'>
        You are already registered<br/>
        <Link to="/">Go to Index Page</Link>
    </div>

export default AlreadyRegistered;