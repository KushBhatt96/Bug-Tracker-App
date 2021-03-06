import React from 'react';
import {Tab} from 'semantic-ui-react';
import ProfilePhotos from './ProfilePhotos';
import ProfileDescription from './ProfileDescription';

/*
 * User profile that contains panels that hold aspects of a user profile
 */

const panes = [
    {menuItem: 'About', render: () => <ProfileDescription/>},
    {menuItem: 'Photos', render: () => <ProfilePhotos/>},
];

interface IProps {
    setActiveTab: (activeIndex: any) => void;
}

const ProfileContent: React.FC<IProps> = ({setActiveTab}) => {
    return (
        <Tab
            menu={{fluid: true, vertical: true}}
            menuPosition='right'
            panes={panes}
            onTabChange={(e, data) => setActiveTab(data.activeIndex)}
        />
    );
};

export default ProfileContent;
