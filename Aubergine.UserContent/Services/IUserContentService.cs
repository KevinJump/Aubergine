using System;
using System.Collections.Generic;
using Aubergine.UserContent.Models;
using Umbraco.Core;

namespace Aubergine.UserContent.Services
{
    public interface IUserContentService
    {
        /// <summary>
        ///  Retreive all user content connected to this node by node.key
        /// </summary>
        IEnumerable<IUserContent> GetByContentKey(Guid contentKey, bool getAll = false);

        /// <summary>
        ///  Retreive all user content connected to this node by node.key, based on status
        /// </summary>
        IEnumerable<IUserContent> GetByContentKey(Guid contentKey, UserContentStatus stauts = UserContentStatus.Approved, bool getAll = false);

        /// <summary>
        ///  Get a single peice of user content
        /// </summary>
        IUserContent Get(Guid key);

        /// <summary>
        /// Save user content to persistant storage 
        /// </summary>
        Attempt<IUserContent> Save(IUserContent content);

        /// <summary>
        /// Remove user content from persistant storage
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        Attempt<IUserContent> Delete(IUserContent content);

        /// <summary>
        /// get all the children content for a give peice of user content
        /// </summary>
        IEnumerable<IUserContent> GetChildren(Guid key, bool getAll = false);

        /// <summary>
        /// get all the children content for a give peice of user content
        /// </summary>
        IEnumerable<IUserContent> GetChildren(Guid key, UserContentStatus status = UserContentStatus.Approved, bool getAll = false);

        /// <summary>
        /// Set the status of the content
        /// </summary>
        bool SetStatus(Guid key, UserContentStatus status);
    }
}