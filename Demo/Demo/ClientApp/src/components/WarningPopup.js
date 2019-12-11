import React from 'react';
import { Modal } from 'react-bootstrap';

const WarningPopup = (props) => {
    return (
        <Modal show={props.show} size="lg" aria-labelledby="contained-modal-title-vcenter" centered="true">
            <Modal.Body dangerouslySetInnerHTML={{ __html: props.message }}>

            </Modal.Body>
            <Modal.Footer>
                <input type="button" className="cus-button primary" value="OK" onClick={props.popupClose} />
            </Modal.Footer>
        </Modal>)
}

export default WarningPopup;