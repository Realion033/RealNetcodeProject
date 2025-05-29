using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Http;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;
using NetworkEvent = Unity.Networking.Transport.NetworkEvent;

namespace NoNameGun
{
    public class MultiNetcodeManager : MonoBehaviour
    {
        const int _maxConnections = 2;
        public string RelayJoinCode;

        public async void AuthenticationPlayer()
        {
            try
            {
                await UnityServices.InitializeAsync();
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                var playerID = AuthenticationService.Instance.PlayerId;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                NetworkManager.Singleton.StartHost();
            }
        }
    }
}
