namespace GB
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="ProgressReport.cs" company="GB">
    /// The MIT License (MIT)
    /// 
    /// Copyright (c) 2014-2020 GB
    /// 
    /// Permission is hereby granted, free of charge, to any person obtaining a copy
    /// of this software and associated documentation files (the "Software"), to deal
    /// in the Software without restriction, including without limitation the rights
    /// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    /// copies of the Software, and to permit persons to whom the Software is
    /// furnished to do so, subject to the following conditions:
    /// 
    /// The above copyright notice and this permission notice shall be included in
    /// all copies or substantial portions of the Software.
    /// 
    /// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    /// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    /// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    /// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    /// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    /// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    /// THE SOFTWARE.
    /// </copyright>
    /// <list type="table">
    /// <listheader>
    /// <term>Revision</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>
    /// <strong>Date:</strong> 10/2/2018<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>
    /// Initial version.
    /// </description>
    /// </item>
    /// <item>
    /// <term>
    /// <strong>Version:</strong> 0.1.0-preview.1<br/>
    /// <strong>Date:</strong> 3/25/2020<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>
    /// Converted the class to a package.
    /// </description>
    /// </item>
    /// <item>
    /// <term>
    /// <strong>Version:</strong> 0.1.4-preview.1<br/>
    /// <strong>Date:</strong> 5/27/2020<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>
    /// Updating documentation to be compatible with DocFX.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// Helper method to report progress reports.  Thread-safe.
    /// <seealso cref="ThreadSafeInt"/>
    /// </summary>
    public class ProgressReport
    {
        readonly ThreadSafeLong currentStep;
        readonly ThreadSafeLong totalSteps;

        /// <summary>
        /// Constructor to set <see cref="TotalSteps"/>.
        /// </summary>
        /// <param name="totalSteps">Sets <see cref="TotalSteps"/>.</param>
        /// <exception cref="System.ArgumentException">
        /// If <paramref name="totalSteps"/> is less than 1.
        /// </exception>
        public ProgressReport(int totalSteps = 1)
        {
            if (totalSteps < 1)
            {
                throw new System.ArgumentException("Argument \"numberOfSteps\" cannot be less than 1.");
            }
            currentStep = new ThreadSafeLong(0);
            this.totalSteps = new ThreadSafeLong(totalSteps);
        }

        /// <summary>
        /// Amount of progress made, out of <see cref="TotalSteps"/>.
        /// </summary>
        public long CurrentStep
        {
            get
            {
                return currentStep.Value;
            }
            set
            {
                // Don't change the finalValue if value is below 0
                long finalValue = 0;
                if (value > 0)
                {
                    // Grab the number of steps only once
                    finalValue = TotalSteps;

                    // Don't change the finalValue if the value is above NumberOfSteps
                    if (value < finalValue)
                    {
                        finalValue = value;
                    }
                }
                currentStep.Value = finalValue;
            }
        }

        /// <summary>
        /// The total number of steps to make progress in.
        /// </summary>
        public long TotalSteps
        {
            get
            {
                return totalSteps.Value;
            }
        }

        /// <summary>
        /// Progress made, as a fraction between 0 and 1.
        /// </summary>
        public float ProgressPercent
        {
            get
            {
                float returnPercent = CurrentStep;
                returnPercent /= TotalSteps;
                return returnPercent;
            }
        }

        /// <summary>
        /// Sets <see cref="CurrentStep"/> to 0.
        /// </summary>
        public void Reset()
        {
            currentStep.Value = 0;
        }

        /// <summary>
        /// Resets current step, then sets <see cref="TotalSteps"/>.
        /// </summary>
        /// <param name="newTotalSteps">Sets <see cref="TotalSteps"/></param>
        /// <exception cref="System.ArgumentException">
        /// If <paramref name="newTotalSteps"/> is less than 1.
        /// </exception>
        public void SetTotalSteps(long newTotalSteps)
        {
            if (newTotalSteps < 1)
            {
                throw new System.ArgumentException("Argument \"newNumberOfSteps\" cannot be less than 1.");
            }
            Reset();
            totalSteps.Value = newTotalSteps;
        }

        /// <summary>
        /// Increases <see cref="CurrentStep"/> by 1.
        /// </summary>
        public void IncrementCurrentStep()
        {
            if (CurrentStep < TotalSteps)
            {
                currentStep.Increment();
            }
        }
    }
}
